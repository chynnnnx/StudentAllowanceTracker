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
        [Inject] protected IBudgetService BudgetService { get; set; } = default!;

        [Parameter] public CategoryDTO? Category { get; set; }

        protected CategoryDTO CategoryForm { get; set; } = new();
        protected bool IsEditing => Category != null;
        protected bool isSaving = false;
        protected decimal remainingBudget = 0;
        protected BudgetDTO? currentBudget = null;
        protected decimal needsPercentage => currentBudget?.NeedsPercentage ?? 50;
        protected decimal wantsPercentage => currentBudget?.WantsPercentage ?? 30;
        protected decimal savingsPercentage => currentBudget?.SavingsPercentage ?? 20;


        protected override async Task OnInitializedAsync()
        {
            if (IsEditing && Category != null)
            {
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
                CategoryForm = new CategoryDTO
                {
                    Type = CategoryType.Needs
                };
            }

            await CalculateRemainingBudget();
        }

        protected async Task CalculateRemainingBudget()
        {
            var budgets = await BudgetService.GetBudgetsByUser();
            var budget = budgets.FirstOrDefault();
            currentBudget = budget;

            if (budget == null)
            {
                Snackbar.Add("No budget found for user", Severity.Warning);
                remainingBudget = 0;
                return;
            }

            var allCategories = await CategoryService.GetAllCategories();

            var existingCategories = allCategories
                .Where(c => c.Type == CategoryForm.Type &&
                           c.UserID == budget.UserID &&
                           (IsEditing ? c.CategoryID != CategoryForm.CategoryID : true))
                .ToList();

            var usedBudget = existingCategories.Sum(c => c.BudgetAmount ?? 0);

            var totalBudgetForType = CategoryForm.Type switch
            {
                CategoryType.Needs => budget.NeedsBudget,
                CategoryType.Wants => budget.WantsBudget,
                CategoryType.Savings => budget.SavingsBudget,
                _ => 0
            };

            if (IsEditing && Category?.BudgetAmount.HasValue == true)
            {
                remainingBudget = totalBudgetForType - usedBudget;
            }
            else
            {
                remainingBudget = totalBudgetForType - usedBudget;
            }

            remainingBudget = Math.Max(0, remainingBudget);
        }

        protected decimal GetRemainingBudgetForType() => remainingBudget;

        protected async Task Submit()
        {
            if (!IsFormValid())
            {
                Snackbar.Add("Please enter a category name", Severity.Error);
                return;
            }

            if (CategoryForm.BudgetAmount.HasValue && CategoryForm.BudgetAmount.Value > remainingBudget)
            {
                Snackbar.Add($"Budget amount exceeds remaining budget of ₱{remainingBudget:N2}", Severity.Error);
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
                    Snackbar.Add("Failed to save category. Please check if you exceeded the budget limit.", Severity.Error);
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

        protected string GetTypeCardClass(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var cssClass = GetTypeCssClass(type);

            if (isSelected)
            {
                return $"p-4 cursor-pointer transition-all border-2 {cssClass}-border {cssClass}-bg";
            }
            return "p-4 cursor-pointer transition-all border-2 border-gray-300 bg-white";
        }

        protected string GetTypeIconClass(CategoryType type)
        {
            var isSelected = CategoryForm.Type == type;
            var cssClass = GetTypeCssClass(type);
            return $"{cssClass}-color mb-2" + (isSelected ? "" : " opacity-40");
        }

        protected string GetTypeCssClass(CategoryType type)
        {
            return type switch
            {
                CategoryType.Needs => "needs",
                CategoryType.Wants => "wants",
                CategoryType.Savings => "savings",
                _ => "needs"
            };
        }
    }
}   