using Microsoft.AspNetCore.Components;
using StudentAllowanceTracker.Client.Services.Interfaces;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class DashboardBase : LayoutComponentBase
    {
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;
        [Inject] protected IBudgetService BudgetService { get; set; } = default!;
        [Inject] protected IGoalService GoalService { get; set; } = default!;
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected List<AllowanceDTO> allowances = new();
        protected List<ExpenseDTO> expenses = new();
        protected List<BudgetDTO> budgets = new();
        protected List<GoalDTO> goals = new();
        protected List<CategoryDTO> categories = new();
        protected bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected async Task LoadData()
        {
            isLoading = true;
            try
            {
                var allowanceTask = AllowanceService.GetAllowanceByUser();
                var expenseTask = ExpenseService.GetExpensesByUser();
                var budgetTask = BudgetService.GetBudgetsByUser();
                var goalTask = GoalService.GetGoalsByUser();
                var categoryTask = CategoryService.GetAllCategories();

                await Task.WhenAll(allowanceTask, expenseTask, budgetTask, goalTask, categoryTask);

                allowances = (await allowanceTask) ?? new();
                expenses = (await expenseTask) ?? new();
                budgets = (await budgetTask) ?? new();
                goals = (await goalTask) ?? new();
                categories = (await categoryTask) ?? new();
            }
            catch (Exception ex)
            {
                Snackbar.Add("Failed to load dashboard data", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        protected decimal GetTotalBalance()
        {
            var totalAllowance = allowances.Where(IsActive).Sum(a => a.Amount);
            var totalSpent = expenses.Sum(e => e.Amount);
            return totalAllowance - totalSpent;
        }

        protected decimal GetTotalSpentThisMonth()
        {
            var now = DateTime.Now;
            return expenses
                .Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                .Sum(e => e.Amount);
        }

        protected decimal GetTotalIncome()
        {
            return allowances.Where(IsActive).Sum(a => a.Amount);
        }

        protected List<ExpenseDTO> GetRecentExpenses(int count = 5)
        {
            return expenses.OrderByDescending(e => e.Date).Take(count).ToList();
        }

        protected BudgetDTO? GetActiveBudget()
        {
            var today = DateTime.Today;
            return budgets.FirstOrDefault(b =>
                b.StartDate <= today &&
                (!b.EndDate.HasValue || b.EndDate.Value >= today));
        }

        protected decimal GetSpentByType(CategoryType type)
        {
            var categoryIds = categories
                .Where(c => c.Type == type)
                .Select(c => c.CategoryID)
                .ToList();

            var now = DateTime.Now;
            return expenses
                .Where(e => categoryIds.Contains(e.CategoryID) &&
                       e.Date.Month == now.Month &&
                       e.Date.Year == now.Year)
                .Sum(e => e.Amount);
        }

        protected int GetBudgetPercentage(CategoryType type, decimal budgetAmount)
        {
            if (budgetAmount == 0) return 0;
            var spent = GetSpentByType(type);
            return (int)Math.Min((spent / budgetAmount) * 100, 100);
        }

        protected int GetGoalPercentage(Guid goalId)
        {
            var goal = goals.FirstOrDefault(g => g.GoalID == goalId);
            if (goal == null || goal.TargetAmount == 0) return 0;

            return (int)Math.Min((goal.CurrentAmount / goal.TargetAmount) * 100, 100);
        }

        protected bool IsActive(AllowanceDTO allowance)
        {
            if (!allowance.EndDate.HasValue) return true;
            return allowance.EndDate.Value >= DateTime.Today;
        }

        protected string GetCategoryIcon(CategoryType type)
        {
            return type switch
            {
                CategoryType.Needs => Icons.Material.Filled.ShoppingBasket,
                CategoryType.Wants => Icons.Material.Filled.Favorite,
                CategoryType.Savings => Icons.Material.Filled.Savings,
                _ => Icons.Material.Filled.Category
            };
        }

        protected Color GetProgressColor(int percentage)
        {
            if (percentage >= 90) return Color.Error;
            if (percentage >= 70) return Color.Warning;
            return Color.Success;
        }

        protected string GetCategoryName(Guid categoryId)
        {
            return categories.FirstOrDefault(c => c.CategoryID == categoryId)?.CategoryName ?? "Unknown";
        }
    }
}