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

        protected List<CategoryDTO> categories = new();
        protected List<CategoryDTO> needsCategories => categories.Where(c => c.Type == CategoryType.Needs).ToList();
        protected List<CategoryDTO> wantsCategories => categories.Where(c => c.Type == CategoryType.Wants).ToList();
        protected List<CategoryDTO> savingsCategories => categories.Where(c => c.Type == CategoryType.Savings).ToList();

        protected decimal needsTotal => needsCategories.Sum(c => c.BudgetAmount ?? 0);
        protected decimal wantsTotal => wantsCategories.Sum(c => c.BudgetAmount ?? 0);
        protected decimal savingsTotal => savingsCategories.Sum(c => c.BudgetAmount ?? 0);

        // Pagination
        protected Dictionary<CategoryType, int> currentPages = new()
        {
            { CategoryType.Needs, 0 },
            { CategoryType.Wants, 0 },
            { CategoryType.Savings, 0 }
        };
        protected const int itemsPerPage = 3;

        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();
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

        protected (string color, string icon, string bgColor) GetTypeStyle(CategoryType type)
        {
            return type switch
            {
                CategoryType.Needs => ("hsl(162, 86.6%, 32.2%)", Icons.Material.Filled.Home, "hsl(162, 41.8%, 95%)"),
                CategoryType.Wants => ("hsl(280, 86.6%, 60%)", Icons.Material.Filled.Favorite, "hsl(280, 41.8%, 95%)"),
                CategoryType.Savings => ("hsl(210, 86.6%, 50%)", Icons.Material.Filled.Savings, "hsl(210, 41.8%, 95%)"),
                _ => ("hsl(162, 86.6%, 32.2%)", Icons.Material.Filled.Category, "hsl(162, 41.8%, 95%)")
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
                { "Category", category }  // Changed from "CategoryForm" to "Category"
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