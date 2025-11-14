using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;
namespace StudentAllowanceTracker.Client.Components.Dialogs.User
{
    public class ExpenseDialogBase: LayoutComponentBase
    {
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected ICategoryService CategoryService { get; set; } = default!;

        [Parameter] public ExpenseDTO? Expense { get; set; }
        protected List<CategoryDTO> categories = new();


        protected string description = string.Empty;
        protected decimal amount;
        protected string category = "Food";
        protected string customCategory = string.Empty;
        protected DateTime? date = DateTime.Today;
        protected bool isSaving;
        protected bool IsEditMode => Expense != null;

        protected Guid selectedAllowanceId;
        protected List<AllowanceDTO> allowances = new();
        protected decimal totalAllowance;
        protected record CategoryItem(string Name, string Icon, string Color);


    

        protected override async Task OnInitializedAsync()
        {
            // Load allowances
            allowances = await AllowanceService.GetAllowanceByUser();
            totalAllowance = allowances.Sum(a => a.Amount);

            // Load categories from CategoryService
            await LoadCategories();

            if (IsEditMode && Expense != null)
            {
                description = Expense.Description;
                amount = Expense.Amount;
                category = Expense.Category;
                date = Expense.Date;
                selectedAllowanceId = Expense.AllowanceID;

                if (!categories.Any(c => c.CategoryName == Expense.Category))
                    customCategory = Expense.Category;
            }
        }

        private async Task LoadCategories()
        {
            try
            {
                categories = await CategoryService.GetAllCategories();

                // Ensure "Other" option exists for custom categories
                if (!categories.Any(c => c.CategoryName == "Other"))
                {
                    categories.Add(new CategoryDTO
                    {
                        CategoryID = Guid.Empty,
                        CategoryName = "Other",
                        Type = CategoryType.Needs, // type doesn’t matter here
                    });
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to load categories: {ex.Message}", Severity.Error);
            }
        }


        protected async Task Submit()
        {
            if (selectedAllowanceId == Guid.Empty)
            {
                Snackbar.Add("Please select an allowance", Severity.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                Snackbar.Add("Description is required", Severity.Error);
                return;
            }

            if (amount <= 0)
            {
                Snackbar.Add("Amount must be greater than zero", Severity.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                Snackbar.Add("Category is required", Severity.Error);
                return;
            }

            if (category == "Other" && string.IsNullOrWhiteSpace(customCategory))
            {
                Snackbar.Add("Please specify a category name", Severity.Error);
                return;
            }

            if (date == null)
            {
                Snackbar.Add("Date is required", Severity.Error);
                return;
            }

            try
            {
                isSaving = true;

                var finalCategory = category == "Other" ? customCategory.Trim() : category;

                var dto = new ExpenseDTO
                {
                    ExpenseID = Expense?.ExpenseID ?? Guid.Empty,
                    AllowanceID = selectedAllowanceId,
                    Description = description.Trim(),
                    Amount = amount,
                    Category = finalCategory,
                    Date = date!.Value
                };

                if (IsEditMode)
                {
                    await ExpenseService.UpdateExpense(dto);
                    Snackbar.Add("Expense updated successfully", Severity.Success);
                }
                else
                {
                    await ExpenseService.AddExpense(dto);
                    Snackbar.Add("Expense added successfully", Severity.Success);
                }

                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to save expense: {ex.Message}", Severity.Error);
            }
            finally
            {
                isSaving = false;
            }
        }

        protected void Cancel() => MudDialog.Cancel();

    }
}
