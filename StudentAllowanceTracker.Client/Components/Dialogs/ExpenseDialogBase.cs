using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
namespace StudentAllowanceTracker.Client.Components.Dialogs
{
    public class ExpenseDialogBase: LayoutComponentBase
    {
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IExpenseService ExpenseService { get; set; } = default!;
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;

        [Parameter] public ExpenseDTO? Expense { get; set; }

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


        protected List<CategoryItem> categories = new()
    {
        new("Food", Icons.Material.Filled.Restaurant, "hsl(25, 95%, 53%)"),
        new("Transportation", Icons.Material.Filled.DirectionsBus, "hsl(217, 91%, 60%)"),
        new("Education", Icons.Material.Filled.School, "hsl(162, 86.6%, 32.2%)"),
        new("Entertainment", Icons.Material.Filled.Movie, "hsl(291, 47%, 51%)"),
        new("Shopping", Icons.Material.Filled.ShoppingBag, "hsl(340, 82%, 52%)"),
        new("Bills", Icons.Material.Filled.Receipt, "hsl(45, 93%, 47%)"),
        new("Healthcare", Icons.Material.Filled.LocalHospital, "hsl(4, 90%, 58%)"),
        new("Other", Icons.Material.Filled.Category, "hsl(0, 0%, 52.2%)")
    };

        protected override async Task OnInitializedAsync()
        {
            allowances = await AllowanceService.GetAllowanceByUser();
            totalAllowance = allowances.Sum(a => a.Amount);

            if (IsEditMode && Expense != null)
            {
                description = Expense.Description;
                amount = Expense.Amount;
                category = Expense.Category;
                date = Expense.Date;
                selectedAllowanceId = Expense.AllowanceID;

                if (!categories.Any(c => c.Name == Expense.Category))
                    customCategory = Expense.Category;
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
