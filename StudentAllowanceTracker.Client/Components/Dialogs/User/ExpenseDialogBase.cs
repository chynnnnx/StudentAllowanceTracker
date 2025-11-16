using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.Components.Dialogs.User
{
    public class ExpenseDialogBase : LayoutComponentBase
    {
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;

        [Parameter] public ExpenseDTO? Expense { get; set; }
        protected DateTime? TempDate { get; set; } = DateTime.Today;

        protected ExpenseDTO Model { get; set; } = new();
        protected List<CategoryDTO> categories = new();
        protected List<AllowanceDTO> allowances = new();
        protected List<ExpenseDTO> allExpenses = new();
        protected AllowanceDTO? SelectedAllowance { get; set; }

        protected decimal totalAllowance;
        protected decimal totalAllowanceBalance; // Remaining balance after expenses
        protected bool isSaving;
        protected bool IsEditMode => Expense != null;

        protected CategoryType SelectedCategoryType { get; set; } = CategoryType.Needs;
        protected CategoryDTO? SelectedCategory { get; set; }

        // Budget tracking
        protected decimal currentCategorySpending = 0;
        protected bool showBudgetWarning = false;
        protected string budgetWarningMessage = "";

        protected IEnumerable<CategoryDTO> FilteredCategories =>
            categories.Where(c => c.Type == SelectedCategoryType);

        protected override async Task OnInitializedAsync()
        {
            // Load all data
            allowances = await AllowanceService.GetAllowanceByUser();
            categories = await CategoryService.GetAllCategories();
            allExpenses = await ExpenseService.GetExpensesByUser() ?? new List<ExpenseDTO>();
            totalAllowance = allowances.Sum(a => a.Amount);

            Model ??= new ExpenseDTO();

            if (IsEditMode && Expense != null)
            {
                Model = new ExpenseDTO
                {
                    ExpenseID = Expense.ExpenseID,
                    AllowanceID = Expense.AllowanceID,
                    Amount = Expense.Amount,
                    Description = Expense.Description,
                    CategoryID = Expense.CategoryID,
                    Date = Expense.Date
                };

                TempDate = Expense.Date;
                SelectedAllowance = allowances.FirstOrDefault(a => a.AllowanceID == Expense.AllowanceID);
                SelectedCategory = categories.FirstOrDefault(c => c.CategoryID == Expense.CategoryID);

                if (SelectedCategory != null)
                {
                    SelectedCategoryType = SelectedCategory.Type;
                    CheckCategoryBudget();
                }
            }
            else
            {
                Model.Date = DateTime.Today;
                TempDate = DateTime.Today;
                SelectedCategory = null;
                SelectedAllowance = null;
            }
        }

        protected void OnCategoryChanged()
        {
            if (SelectedCategory != null)
            {
                CheckCategoryBudget();
            }
        }

        protected void OnAmountChanged()
        {
            if (SelectedCategory != null && Model.Amount > 0)
            {
                CheckCategoryBudget();
            }
        }

        protected void CheckCategoryBudget()
        {
            if (SelectedCategory == null || !SelectedCategory.BudgetAmount.HasValue)
            {
                showBudgetWarning = false;
                return;
            }

            // Filter expenses by category (exclude current if editing)
            var categoryExpenses = allExpenses
                .Where(e => e.CategoryID == SelectedCategory.CategoryID && e.ExpenseID != Model.ExpenseID)
                .ToList();

            currentCategorySpending = categoryExpenses.Sum(e => e.Amount);
            var projectedSpending = currentCategorySpending + Model.Amount;
            var budget = SelectedCategory.BudgetAmount.Value;

            if (projectedSpending > budget)
            {
                showBudgetWarning = true;
                var excess = projectedSpending - budget;
                var percentage = (projectedSpending / budget) * 100;

                budgetWarningMessage = $"Warning! This expense will exceed your '{SelectedCategory.CategoryName}' budget by ₱{excess:N2} ({percentage:F0}% of budget).";
            }
            else if (projectedSpending >= budget * 0.8m) // 80% threshold
            {
                showBudgetWarning = true;
                var percentage = (projectedSpending / budget) * 100;
                var remaining = budget - projectedSpending;

                budgetWarningMessage = $"Approaching budget limit! You'll have ₱{remaining:N2} remaining ({percentage:F0}% used).";
            }
            else
            {
                showBudgetWarning = false;
            }
        }

        protected async Task Submit()
        {
            if (SelectedAllowance == null ||
                string.IsNullOrWhiteSpace(Model.Description) ||
                Model.Amount <= 0 ||
                SelectedCategory == null ||
                TempDate == null)
            {
                Snackbar.Add("Complete all required fields", Severity.Error);
                return;
            }

            // Check allowance balance
            var allowanceExpenses = allExpenses
                .Where(e => e.AllowanceID == SelectedAllowance.AllowanceID && e.ExpenseID != Model.ExpenseID)
                .Sum(e => e.Amount);

            var availableBalance = SelectedAllowance.Amount - allowanceExpenses;

            if (Model.Amount > availableBalance)
            {
                Snackbar.Add($"Insufficient allowance balance. Available: ₱{availableBalance:N2}", Severity.Error);
                return;
            }

            isSaving = true;

            Model.AllowanceID = SelectedAllowance.AllowanceID;
            Model.CategoryID = SelectedCategory.CategoryID;
            Model.Date = TempDate.Value;
            Model.ExpenseID = Expense?.ExpenseID ?? Guid.Empty;

            bool success = false;

            try
            {
                if (IsEditMode)
                {
                    var updated = await ExpenseService.UpdateExpense(Model);
                    success = updated != null;
                }
                else
                {
                    success = await ExpenseService.AddExpense(Model);
                }

                if (success)
                {
                    Snackbar.Add(IsEditMode ? "Expense updated successfully" : "Expense added successfully", Severity.Success);

                    // Show budget notification if exceeded
                    if (showBudgetWarning && SelectedCategory?.BudgetAmount.HasValue == true)
                    {
                        var projectedSpending = currentCategorySpending + Model.Amount;
                        if (projectedSpending > SelectedCategory.BudgetAmount.Value)
                        {
                            Snackbar.Add(
                                $"Budget exceeded for '{SelectedCategory.CategoryName}'! Total spent: ₱{projectedSpending:N2} / ₱{SelectedCategory.BudgetAmount.Value:N2}",
                                Severity.Warning,
                                config => { config.VisibleStateDuration = 5000; }
                            );
                        }
                    }

                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add("Failed to save expense. Please try again.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                isSaving = false;
            }
        }

        protected void Cancel() => MudDialog.Cancel();

        protected string GetBudgetProgressColor()
        {
            if (!SelectedCategory?.BudgetAmount.HasValue ?? true) return "info";

            var projected = currentCategorySpending + Model.Amount;
            var percentage = (projected / SelectedCategory.BudgetAmount.Value) * 100;

            if (percentage >= 100) return "error";
            if (percentage >= 80) return "warning";
            return "success";
        }

        protected Color GetBudgetProgressMudColor()
        {
            var colorStr = GetBudgetProgressColor();
            return colorStr switch
            {
                "error" => Color.Error,
                "warning" => Color.Warning,
                "success" => Color.Success,
                _ => Color.Info
            };
        }

        protected double GetBudgetProgressPercentage()
        {
            if (!SelectedCategory?.BudgetAmount.HasValue ?? true) return 0;

            var projected = currentCategorySpending + Model.Amount;
            var percentage = (double)((projected / SelectedCategory.BudgetAmount.Value) * 100);

            return Math.Min(percentage, 100);
        }
    }
}