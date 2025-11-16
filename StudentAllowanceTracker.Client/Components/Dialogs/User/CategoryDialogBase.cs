using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.Components.Dialogs.User
{
    public class CategoryDialogBase : LayoutComponentBase
    {
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        [Parameter] public CategoryDTO? Category { get; set; }

        protected CategoryDTO CategoryForm { get; set; } = new();
        protected bool IsEditing => Category != null;
        protected bool isSaving = false;

        protected override void OnInitialized()
        {
            if (IsEditing && Category != null)
            {
                // Copy values from the passed category
                CategoryForm = new CategoryDTO
                {
                    CategoryID = Category.CategoryID,
                    UserID = Category.UserID,
                    CategoryName = Category.CategoryName,
                    Type = Category.Type,
                    BudgetAmount = Category.BudgetAmount
                };
            }
            else
            {
                // New category
                CategoryForm = new CategoryDTO
                {
                    Type = CategoryType.Needs
                };
            }
        }

        protected async Task Submit()
        {
            if (!IsFormValid())
            {
                Snackbar.Add("Please enter a category name", Severity.Error);
                return;
            }

            isSaving = true;
            bool success = false;

            try
            {
                if (IsEditing)
                {
                    success = await CategoryService.UpdateCategory(CategoryForm.CategoryID, CategoryForm);
                }
                else
                {
                    success = await CategoryService.AddCategory(CategoryForm);
                }

                if (success)
                {
                    Snackbar.Add(IsEditing ? "Category updated successfully" : "Category added successfully", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add("Failed to save category", Severity.Error);
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

        protected void Cancel()
        {
            MudDialog.Cancel();
        }

        protected bool IsFormValid()
        {
            return !string.IsNullOrWhiteSpace(CategoryForm.CategoryName);
        }

        protected string GetTypeCardStyle(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var (color, _, bgColor) = GetTypeStyle(type);

            if (isSelected)
            {
                return $"border-color: {color}; background-color: {bgColor}; border-radius: 0.75rem;";
            }
            return $"border-color: hsl(0, 0%, 85%); background-color: white; border-radius: 0.75rem;";
        }

        protected string GetTypeIconStyle(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var (color, _, _) = GetTypeStyle(type);
            return $"color: {color}; font-size: 2rem; opacity: {(isSelected ? "1" : "0.4")};";
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
    }
}