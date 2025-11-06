using Microsoft.AspNetCore.Components;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Dialogs;
namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class ExpensesBase: LayoutComponentBase
    {

        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;

        [Inject] protected IDialogService DialogService { get; set; } = default!;

        protected List<ExpenseDTO> expenses = new();
        protected bool isLoading;
        protected string? selectedCategory;
        protected DateRange? dateRange;

        protected override async Task OnInitializedAsync() => await LoadExpenses();

        protected async Task LoadExpenses()
        {
            try
            {
                isLoading = true;
                var result = await ExpenseService.GetExpensesByUser();
                expenses = result?.OrderByDescending(e => e.Date).ToList() ?? new();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to load expenses: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task OpenAddDialog()
        {
            var dialog = await DialogService.ShowAsync<ExpenseDialog>(
                "Add Expense",
                new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true }
            );

            var result = await dialog.Result;
            if (!result.Canceled)
                await LoadExpenses();
        }

        protected async Task OpenEditDialog(ExpenseDTO expense)
        {
            var parameters = new DialogParameters { ["Expense"] = expense };
            var dialog = await DialogService.ShowAsync<ExpenseDialog>(
                "Edit Expense",
                parameters,
                new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true }
            );

            var result = await dialog.Result;
            if (!result.Canceled)
                await LoadExpenses();
        }

        protected async Task DeleteExpense(ExpenseDTO expense)
        {
            bool? confirm = await DialogService.ShowMessageBox(
                "Delete Expense",
                $"Are you sure you want to delete '{expense.Description}'?",
                yesText: "Delete",
                cancelText: "Cancel");

            if (confirm == true)
            {
                await ExpenseService.DeleteExpense(expense.ExpenseID);
                Snackbar.Add("Expense deleted successfully", Severity.Success);
                await LoadExpenses();
            }
        }

        protected List<ExpenseDTO> GetFilteredExpenses()
        {
            var filtered = expenses.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedCategory))
                filtered = filtered.Where(e => e.Category == selectedCategory);

            if (dateRange?.Start != null && dateRange?.End != null)
                filtered = filtered.Where(e => e.Date >= dateRange.Start && e.Date <= dateRange.End);

            return filtered.OrderByDescending(e => e.Date).ToList();
        }

        protected decimal GetThisMonthTotal() =>
            expenses.Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                     .Sum(e => e.Amount);

        protected decimal GetTodayTotal() =>
            expenses.Where(e => e.Date.Date == DateTime.Today)
                     .Sum(e => e.Amount);

        protected string GetCategoryColor(string category) => category.ToLower() switch
        {
            "food" => "hsl(25, 95%, 53%)",
            "transportation" => "hsl(217, 91%, 60%)",
            "education" => "hsl(162, 86.6%, 32.2%)",
            "entertainment" => "hsl(291, 47%, 51%)",
            "shopping" => "hsl(340, 82%, 52%)",
            "bills" => "hsl(45, 93%, 47%)",
            "healthcare" => "hsl(4, 90%, 58%)",
            _ => "hsl(0, 0%, 52.2%)"
        };

        protected string GetCategoryIcon(string category) => category.ToLower() switch
        {
            "food" => Icons.Material.Filled.Restaurant,
            "transportation" => Icons.Material.Filled.DirectionsBus,
            "education" => Icons.Material.Filled.School,
            "entertainment" => Icons.Material.Filled.Movie,
            "shopping" => Icons.Material.Filled.ShoppingBag,
            "bills" => Icons.Material.Filled.Receipt,
            "healthcare" => Icons.Material.Filled.LocalHospital,
            _ => Icons.Material.Filled.Category
        };
    }
}
