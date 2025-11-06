using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Components.Dialogs
{
    public class AllowanceDialogBase: LayoutComponentBase
    {
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;

        [Parameter] public Guid? AllowanceID { get; set; }
        [Parameter] public bool IsEditMode { get; set; }



        protected AllowanceDTO allowance = new()
        {
            StartDate = DateTime.Today,
            Type = AllowanceType.Weekly
        };

        protected bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode && AllowanceID.HasValue)
            {
                await LoadAllowance();
            }
            else
            {
                isLoading = false;
            }
        }

        protected async Task LoadAllowance()
        {
            try
            {
                var allowances = await AllowanceService.GetAllowanceByUser();
                var existing = allowances?.FirstOrDefault(a => a.AllowanceID == AllowanceID.Value);

                if (existing != null)
                {
                    allowance = existing;
                }
                else
                {
                    Snackbar.Add("Allowance not found", Severity.Error);
                    MudDialog.Cancel();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add("Failed to load allowance", Severity.Error);
                MudDialog.Cancel();
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task Submit()
        {
            try
            {
                if (IsEditMode)
                {
                    var result = await AllowanceService.UpdateAllowance(allowance);
                    if (result != null)
                    {
                        Snackbar.Add("Allowance updated successfully!", Severity.Success);
                        MudDialog.Close(DialogResult.Ok(result));
                    }
                    else
                    {
                        Snackbar.Add("Failed to update allowance", Severity.Error);
                    }
                }
                else
                {
                    var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                    var user = authState.User;

                    if (!user.Identity.IsAuthenticated)
                    {
                        Snackbar.Add("You must log in first.", Severity.Warning);
                        return;
                    }

                    var success = await AllowanceService.AddAllowance(allowance);

                    if (success)
                    {
                        Snackbar.Add("Allowance added successfully!", Severity.Success);
                        MudDialog.Close(DialogResult.Ok(success));
                    }
                    else
                    {
                        Snackbar.Add("Failed to add allowance. Please check your connection.", Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }
        }


        protected void Cancel() => MudDialog.Cancel();

        protected bool IsValid() =>
            allowance.Amount > 0 && !string.IsNullOrWhiteSpace(allowance.Description);

        protected string GetFrequencyText() => allowance.Type switch
        {
            AllowanceType.OneTime => "once",
            AllowanceType.Daily => "every day",
            AllowanceType.Weekly => "every week",
            AllowanceType.Monthly => "every month",
            AllowanceType.Yearly => "every year",
            _ => ""
        };

        protected string GetTypeCardStyle(AllowanceType type) =>
            allowance.Type == type
            ? "background-color: hsl(103, 96.4%, 89%); border: 2px solid hsl(162, 86.6%, 32.2%); border-radius: 0.75rem;"
            : "background-color: white; border: 2px solid hsl(162, 41.8%, 89.2%); border-radius: 0.75rem;";

        protected string GetTypeIconStyle(AllowanceType type) =>
            allowance.Type == type
            ? "color: hsl(162, 86.6%, 32.2%);"
            : "color: hsl(0, 0%, 52.2%);";

        protected string GetTypeTextStyle(AllowanceType type) =>
            allowance.Type == type
            ? "color: hsl(0, 0%, 10.6%);"
            : "color: hsl(0, 0%, 52.2%);";

        protected string GetTypeIcon(AllowanceType type) => type switch
        {
            AllowanceType.OneTime => Icons.Material.Filled.EventAvailable,
            AllowanceType.Daily => Icons.Material.Filled.WbSunny,
            AllowanceType.Weekly => Icons.Material.Filled.CalendarViewWeek,
            AllowanceType.Monthly => Icons.Material.Filled.CalendarMonth,
            AllowanceType.Yearly => Icons.Material.Filled.CalendarToday,
            _ => Icons.Material.Filled.CalendarToday
        };
    }
}
