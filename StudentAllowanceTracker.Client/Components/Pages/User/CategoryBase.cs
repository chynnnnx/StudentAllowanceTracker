using Microsoft.AspNetCore.Components;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.Components.Dialogs.User;
using MudBlazor;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class CategoryBase : LayoutComponentBase
    {
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IBudgetService BudgetService { get; set; } = default!;

        protected List<CategoryDTO> categories = new();
        protected BudgetDTO? currentBudget = null;
        protected List<CategoryDTO> needsCategories => categories.Where(c => c.Type == CategoryType.Needs).ToList();
        protected List<CategoryDTO> wantsCategories => categories.Where(c => c.Type == CategoryType.Wants).ToList();
        protected List<CategoryDTO> savingsCategories => categories.Where(c => c.Type == CategoryType.Savings).ToList();

        protected decimal needsTotal => needsCategories.Sum(c => c.BudgetAmount ?? 0);
        protected decimal wantsTotal => wantsCategories.Sum(c => c.BudgetAmount ?? 0);
        protected decimal savingsTotal => savingsCategories.Sum(c => c.BudgetAmount ?? 0);

        protected decimal needsPercentage => currentBudget?.NeedsPercentage ?? 50;
        protected decimal wantsPercentage => currentBudget?.WantsPercentage ?? 30;
        protected decimal savingsPercentage => currentBudget?.SavingsPercentage ?? 20;

        protected Dictionary<CategoryType, int> currentPages = new()
        {
            { CategoryType.Needs, 0 },
            { CategoryType.Wants, 0 },
            { CategoryType.Savings, 0 }
        };
        protected const int itemsPerPage = 3;

        protected override async Task OnInitializedAsync()
        {
            await LoadBudget();
            await LoadCategories();
        }

        protected async Task LoadBudget()
        {
            try
            {
                var budgets = await BudgetService.GetBudgetsByUser();
                currentBudget = budgets.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading budget: {ex.Message}", Severity.Error);
            }
        }

        protected async Task LoadCategories()
        {
            try
            {
                categories = await CategoryService.GetAllCategories();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading categories: {ex.Message}", Severity.Error);
            }
        }

        protected List<CategoryDTO> GetCategoriesByType(CategoryType type)
        {
            return categories.Where(c => c.Type == type).ToList();
        }

        protected List<CategoryDTO> GetPagedCategories(CategoryType type)
        {
            var typeCategories = GetCategoriesByType(type);
            var currentPage = currentPages[type];
            return typeCategories.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
        }

        protected int GetTotalPages(CategoryType type)
        {
            var count = GetCategoriesByType(type).Count;
            return (int)Math.Ceiling(count / (double)itemsPerPage);
        }

        protected void ChangePage(CategoryType type, int newPage)
        {
            var totalPages = GetTotalPages(type);
            if (newPage >= 0 && newPage < totalPages)
            {
                currentPages[type] = newPage;
            }
        }

        protected (string icon, string cssClass) GetTypeStyle(CategoryType type)
        {
            return type switch
            {
                CategoryType.Needs => (Icons.Material.Filled.Home, "needs"),
                CategoryType.Wants => (Icons.Material.Filled.Favorite, "wants"),
                CategoryType.Savings => (Icons.Material.Filled.Savings, "savings"),
                _ => (Icons.Material.Filled.Category, "needs")
            };
        }

        protected async Task OpenAddDialog()
        {
            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await DialogService.ShowAsync<CategoryDialog>("Add Category", options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadCategories();
            }
        }

        protected async Task EditCategory(CategoryDTO category)
        {
            var parameters = new DialogParameters
            {
                { "Category", category }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await DialogService.ShowAsync<CategoryDialog>("Edit Category", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadCategories();
            }
        }

        protected async Task DeleteCategory(CategoryDTO category)
        {
            var parameters = new DialogParameters
            {
                { "Message", $"Are you sure you want to delete '{category.CategoryName}'?" },
                { "ButtonText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowMessageBox(
                "Delete Category",
                $"Are you sure you want to delete '{category.CategoryName}'? This action cannot be undone.",
                yesText: "Delete",
                cancelText: "Cancel"
            );

            if (dialog == true)
            {
                try
                {
                    var success = await CategoryService.DeleteCategory(category.CategoryID);

                    if (success)
                    {
                        Snackbar.Add("Category deleted successfully", Severity.Success);
                        await LoadCategories();
                    }
                    else
                    {
                        Snackbar.Add("Failed to delete category", Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error deleting category: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}