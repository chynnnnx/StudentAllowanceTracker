using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class BudgetBase : LayoutComponentBase
    {
        [Inject] protected IBudgetService BudgetService { get; set; } = default!;
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        protected BudgetDTO? currentBudget = null;
        protected List<AllowanceDTO> allowances = new();
        protected List<ExpenseDTO> expenses = new();
        protected List<CategoryDTO> categories = new();
        protected bool isLoading = false;
        protected bool hasBudget = false;

        protected decimal totalAllowance = 0;
        protected decimal needsPercentage = 50;
        protected decimal wantsPercentage = 30;
        protected decimal savingsPercentage = 20;
        protected DateTime startDate = DateTime.Today;
        protected DateTime? endDate = null;

        protected decimal needsSpent = 0;
        protected decimal wantsSpent = 0;
        protected decimal savingsSpent = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected async Task LoadData()
        {
            isLoading = true;
            try
            {
               
                var budgets = await BudgetService.GetBudgetsByUser();
                currentBudget = budgets.FirstOrDefault();
                hasBudget = currentBudget != null;

                if (hasBudget)
                {
                    needsPercentage = currentBudget!.NeedsPercentage;
                    wantsPercentage = currentBudget.WantsPercentage;
                    savingsPercentage = currentBudget.SavingsPercentage;
                    startDate = currentBudget.StartDate;
                    endDate = currentBudget.EndDate;
                }

               
                allowances = await AllowanceService.GetAllowanceByUser() ?? new List<AllowanceDTO>();
                totalAllowance = allowances.Sum(a => a.Amount);

                categories = await CategoryService.GetAllCategories() ?? new List<CategoryDTO>();

                expenses = await ExpenseService.GetExpensesByUser() ?? new List<ExpenseDTO>();

                CalculateSpending();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        protected void CalculateSpending()
        {
            if (!hasBudget)
            {
                needsSpent = 0;
                wantsSpent = 0;
                savingsSpent = 0;
                return;
            }

            var activeExpenses = expenses.Where(e =>
                e.Date >= startDate &&
                (!endDate.HasValue || e.Date <= endDate.Value)).ToList();

            needsSpent = 0;
            wantsSpent = 0;
            savingsSpent = 0;

            foreach (var expense in activeExpenses)
            {
                var category = categories.FirstOrDefault(c => c.CategoryID == expense.CategoryID);
                if (category != null)
                {
                    switch (category.Type)
                    {
                        case CategoryType.Needs:
                            needsSpent += expense.Amount;
                            break;
                        case CategoryType.Wants:
                            wantsSpent += expense.Amount;
                            break;
                        case CategoryType.Savings:
                            savingsSpent += expense.Amount;
                            break;
                    }
                }
            }
        }

        protected async Task SaveBudget()
        {

            if (needsPercentage + wantsPercentage + savingsPercentage != 100)
            {
                Snackbar.Add("Percentages must add up to 100%", Severity.Warning);
                return;
            }

            if (totalAllowance <= 0)
            {
                Snackbar.Add("Total allowance must be greater than 0", Severity.Warning);
                return;
            }

            try
            {
                var budgetDTO = new BudgetDTO
                {
                    BudgetID = currentBudget?.BudgetID ?? Guid.Empty,
                    TotalAllowance = totalAllowance,
                    NeedsPercentage = needsPercentage,
                    WantsPercentage = wantsPercentage,
                    SavingsPercentage = savingsPercentage,
                    StartDate = startDate,
                    EndDate = endDate
                };

                bool success;
                if (hasBudget)
                {
                    success = await BudgetService.UpdateBudget(currentBudget!.BudgetID, budgetDTO);
                }
                else
                {
                    success = await BudgetService.AddBudget(budgetDTO);
                }

                if (success)
                {
                    Snackbar.Add("Budget saved successfully", Severity.Success);
                    await LoadData();
                }
                else
                {
                    Snackbar.Add("Failed to save budget", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error saving budget: {ex.Message}", Severity.Error);
            }
        }

        protected async Task DeleteBudget()
        {
            if (!hasBudget) return;

            bool? confirm = await DialogService.ShowMessageBox(
                "Delete Budget",
                "Are you sure you want to delete your budget plan?",
                yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    var success = await BudgetService.DeleteBudget(currentBudget!.BudgetID);
                    if (success)
                    {
                        Snackbar.Add("Budget deleted successfully", Severity.Success);
                        await LoadData();
                    }
                    else
                    {
                        Snackbar.Add("Failed to delete budget", Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error deleting budget: {ex.Message}", Severity.Error);
                }
            }
        }

        protected void ResetToDefault()
        {
            needsPercentage = 50;
            wantsPercentage = 30;
            savingsPercentage = 20;
        }

        protected decimal GetBudgetAmount(string type)
        {
            if (!hasBudget) return 0;
            return type switch
            {
                "Needs" => currentBudget!.NeedsBudget,
                "Wants" => currentBudget!.WantsBudget,
                "Savings" => currentBudget!.SavingsBudget,
                _ => 0
            };
        }

        protected decimal GetSpentAmount(string type)
        {
            return type switch
            {
                "Needs" => needsSpent,
                "Wants" => wantsSpent,
                "Savings" => savingsSpent,
                _ => 0
            };
        }

        protected decimal GetRemainingAmount(string type)
        {
            return GetBudgetAmount(type) - GetSpentAmount(type);
        }

        protected double GetSpentPercentage(string type)
        {
            var budget = GetBudgetAmount(type);
            if (budget == 0) return 0;
            return (double)(GetSpentAmount(type) / budget * 100);
        }

        protected string GetProgressColor(string type, double percentage)
        {
            if (percentage >= 90) return "error";
            if (percentage >= 75) return "warning";
            return type switch
            {
                "Needs" => "success",
                "Wants" => "warning",
                "Savings" => "info",
                _ => "default"
            };
        }

        protected string GetCategoryColor(string type)
        {
            return type switch
            {
                "Needs" => "needs-color",
                "Wants" => "wants-color",
                "Savings" => "savings-color",
                _ => "text-secondary"
            };
        }

        protected string GetCategoryIcon(string type)
        {
            return type switch
            {
                "Needs" => Icons.Material.Filled.ShoppingBasket,
                "Wants" => Icons.Material.Filled.ShoppingCart,
                "Savings" => Icons.Material.Filled.Savings,
                _ => Icons.Material.Filled.Category
            };
        }
    }
}