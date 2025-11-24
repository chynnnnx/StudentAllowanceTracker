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

        protected ExpenseDTO Model { get; set; } = new();
        protected DateTime? TempDate { get; set; } = DateTime.Today;

        protected List<CategoryDTO> categories = new();
        protected List<AllowanceDTO> allowances = new();
        protected List<ExpenseDTO> allExpenses = new();
        protected AllowanceDTO? SelectedAllowance { get; set; }
        protected CategoryDTO? SelectedCategory { get; set; }

        protected CategoryType SelectedCategoryType { get; set; } = CategoryType.Needs;

        protected decimal totalAllowance;
        protected decimal totalAllowanceBalance;
        protected decimal currentCategorySpending = 0;

        protected bool isSaving;
        protected bool showBudgetWarning = false;
        protected string budgetWarningMessage = "";

        protected bool IsEditMode => Expense != null;

        protected IEnumerable<CategoryDTO> FilteredCategories => categories.Where(c => c.Type == SelectedCategoryType);

        protected override async Task OnInitializedAsync()
        {
            allowances = await AllowanceService.GetAllowanceByUser();
            categories = await CategoryService.GetAllCategories();
            allExpenses = await ExpenseService.GetExpensesByUser() ?? new List<ExpenseDTO>();

            totalAllowance = allowances.Sum(a => a.Amount);
            UpdateTotalAllowanceBalance();

            if (IsEditMode && Expense != null)
            {
                Model = Expense; 
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
            }
        }

        protected void UpdateTotalAllowanceBalance()
        {
            totalAllowanceBalance = allowances.Sum(a =>
            {
                var spent = allExpenses
                    .Where(e => e.AllowanceID == a.AllowanceID && e.ExpenseID != Model.ExpenseID)
                    .Sum(e => e.Amount);
                return Math.Max(a.Amount - spent, 0);
            });
        }

        protected void OnCategoryChanged()
        {
            if (SelectedCategory != null)
                CheckCategoryBudget();
        }

        protected void OnAmountChanged()
        {
            if (SelectedCategory != null && Model.Amount > 0)
                CheckCategoryBudget();

            UpdateTotalAllowanceBalance();
        }

        protected void OnAllowanceChanged()
        {
            UpdateTotalAllowanceBalance();
        }

        protected void CheckCategoryBudget()
        {
            if (SelectedCategory?.BudgetAmount is not decimal budget)
            {
                showBudgetWarning = false;
                return;
            }

            currentCategorySpending = allExpenses
                .Where(e => e.CategoryID == SelectedCategory.CategoryID && e.ExpenseID != Model.ExpenseID)
                .Sum(e => e.Amount);

            var projected = currentCategorySpending + Model.Amount;

            if (projected > budget)
            {
                showBudgetWarning = true;
                var excess = projected - budget;
                var percentage = (projected / budget) * 100;
                budgetWarningMessage = $"Warning! This expense exceeds '{SelectedCategory.CategoryName}' budget by ₱{excess:N2} ({percentage:F0}%).";
            }
            else if (projected >= budget * 0.8m)
            {
                showBudgetWarning = true;
                var remaining = budget - projected;
                var percentage = (projected / budget) * 100;
                budgetWarningMessage = $"Approaching budget limit! ₱{remaining:N2} remaining ({percentage:F0}% used).";
            }
            else
            {
                showBudgetWarning = false;
            }
        }

        protected async Task Submit()
        {
            if (SelectedAllowance == null || SelectedCategory == null || TempDate == null || string.IsNullOrWhiteSpace(Model.Description) || Model.Amount <= 0)
            {
                Snackbar.Add("Complete all required fields", Severity.Error);
                return;
            }

            var spent = allExpenses
                .Where(e => e.AllowanceID == SelectedAllowance.AllowanceID && e.ExpenseID != Model.ExpenseID)
                .Sum(e => e.Amount);

            if (Model.Amount > SelectedAllowance.Amount - spent)
            {
                Snackbar.Add($"Insufficient allowance balance. Available: ₱{SelectedAllowance.Amount - spent:N2}", Severity.Error);
                return;
            }

            isSaving = true;
            Model.AllowanceID = SelectedAllowance.AllowanceID;
            Model.CategoryID = SelectedCategory.CategoryID;
            Model.Date = TempDate.Value;

            try
            {
                bool success = IsEditMode
                    ? (await ExpenseService.UpdateExpense(Model)) != null
                    : await ExpenseService.AddExpense(Model);

                if (success)
                {
                    Snackbar.Add(IsEditMode ? "Expense updated successfully" : "Expense added successfully", Severity.Success);
                    UpdateTotalAllowanceBalance();

                    if (showBudgetWarning && SelectedCategory?.BudgetAmount.HasValue == true)
                    {
                        var projected = currentCategorySpending + Model.Amount;
                        if (projected > SelectedCategory.BudgetAmount.Value)
                        {
                            Snackbar.Add(
                                $"Budget exceeded for '{SelectedCategory.CategoryName}'! Total spent: ₱{projected:N2} / ₱{SelectedCategory.BudgetAmount.Value:N2}",
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
            if (SelectedCategory?.BudgetAmount is not decimal budget) return "info";
            var projected = currentCategorySpending + Model.Amount;
            var percentage = (projected / budget) * 100;
            return percentage >= 100 ? "error" : percentage >= 80 ? "warning" : "success";
        }

        protected Color GetBudgetProgressMudColor() => GetBudgetProgressColor() switch
        {
            "error" => Color.Error,
            "warning" => Color.Warning,
            "success" => Color.Success,
            _ => Color.Info
        };

        protected double GetBudgetProgressPercentage()
        {
            if (SelectedCategory?.BudgetAmount is not decimal budget) return 0;
            var projected = currentCategorySpending + Model.Amount;
            return Math.Min((double)((projected / budget) * 100), 100);
        }
    }
}
