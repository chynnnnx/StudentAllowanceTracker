using Microsoft.AspNetCore.Components;
using StudentAllowanceTracker.Client.Services.Interfaces;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Dialogs;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Enums;
namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class AllowancesBase: LayoutComponentBase

    {
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected IAllowanceService AllowanceService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected List<AllowanceDTO> allowances = new();
        protected bool isLoading = true;
        protected bool showAll = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadAllowances();
        }

        protected async Task LoadAllowances()
        {
            isLoading = true;
            try
            {
                var result = await AllowanceService.GetAllowanceByUser();
                if (result != null)
                {
                    allowances = result.OrderByDescending(a => a.StartDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add("Failed to load allowances", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task OpenAddDialog()
        {
            var dialog = await DialogService.ShowAsync<AllowanceDialog>("", new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = false
            });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadAllowances();
            }
        }

        protected async Task OpenEditDialog(AllowanceDTO allowance)
        {
            var parameters = new DialogParameters<AllowanceDialog>
        {
            { x => x.AllowanceID, allowance.AllowanceID },
           { x => x.IsEditMode, true }
        };

            var dialog = await DialogService.ShowAsync<AllowanceDialog>("", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseButton = false
            });

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadAllowances();
            }
        }

        protected async Task DeleteAllowance(AllowanceDTO allowance)
        {
            bool? confirm = await DialogService.ShowMessageBox(
                "Delete Allowance",
                $"Are you sure you want to delete '{allowance.Description}'?",
                yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    await AllowanceService.DeleteAllowance(allowance.AllowanceID);
                    Snackbar.Add("Allowance deleted successfully", Severity.Success);
                    await LoadAllowances();
                }
                catch (Exception ex)
                {
                    Snackbar.Add("Failed to delete allowance", Severity.Error);
                }
            }
        }

        protected List<AllowanceDTO> GetFilteredAllowances()
        {
            return showAll ? allowances : allowances.Where(IsActive).ToList();
        }

        protected bool IsActive(AllowanceDTO allowance)
        {
            if (!allowance.EndDate.HasValue) return true;
            return allowance.EndDate.Value >= DateTime.Today;
        }

        protected decimal GetThisMonthTotal()
        {
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var monthEnd = now; // Up to today only
            decimal total = 0;

            foreach (var allowance in allowances.Where(IsActive))
            {
                // Skip if not started yet
                if (allowance.StartDate > now) continue;

                // Get effective start date (either allowance start or month start, whichever is later)
                var effectiveStart = allowance.StartDate > monthStart ? allowance.StartDate : monthStart;

                switch (allowance.Type)
                {
                    case AllowanceType.OneTime:
                        // Only if received this month
                        if (allowance.StartDate.Month == now.Month && allowance.StartDate.Year == now.Year && allowance.StartDate <= now)
                        {
                            total += allowance.Amount;
                        }
                        break;

                    case AllowanceType.Daily:
                        // Count actual days from start to today
                        var days = (now - effectiveStart).Days + 1;
                        total += allowance.Amount * days;
                        break;

                    case AllowanceType.Weekly:
                        // Count actual weeks that passed this month
                        var weeks = (now - effectiveStart).Days / 7;
                        total += allowance.Amount * weeks;
                        break;

                    case AllowanceType.Monthly:
                        // Only count if we've passed the start date this month
                        if (effectiveStart.Month == now.Month && effectiveStart <= now)
                        {
                            total += allowance.Amount;
                        }
                        break;

                    case AllowanceType.Yearly:
                        // Pro-rated based on days passed
                        var yearDays = DateTime.IsLeapYear(now.Year) ? 366 : 365;
                        var daysPassed = now.DayOfYear;
                        total += (allowance.Amount / yearDays) * daysPassed;
                        break;
                }
            }

            return total;
        }

        protected string GetFrequencyIcon(AllowanceType type) => type switch
        {
            AllowanceType.OneTime => Icons.Material.Filled.EventAvailable,
            AllowanceType.Daily => Icons.Material.Filled.WbSunny,
            AllowanceType.Weekly => Icons.Material.Filled.CalendarViewWeek,
            AllowanceType.Monthly => Icons.Material.Filled.CalendarMonth,
            AllowanceType.Yearly => Icons.Material.Filled.CalendarToday,
            _ => Icons.Material.Filled.CalendarToday
        };

        protected string GetFrequencyLabel(AllowanceType type) => type switch
        {
            AllowanceType.OneTime => "One-time",
            AllowanceType.Daily => "Daily",
            AllowanceType.Weekly => "Weekly",
            AllowanceType.Monthly => "Monthly",
            AllowanceType.Yearly => "Yearly",
            _ => ""
        };

        protected string GetFrequencyUnit(AllowanceType type) => type switch
        {
            AllowanceType.OneTime => "one time",
            AllowanceType.Daily => "per day",
            AllowanceType.Weekly => "per week",
            AllowanceType.Monthly => "per month",
            AllowanceType.Yearly => "per year",
            _ => ""
        };

        protected string GetFilterButtonStyle(bool isActive) =>
            isActive
            ? "text-transform: none; color: hsl(162, 86.6%, 32.2%); border-color: hsl(162, 86.6%, 32.2%);"
            : "text-transform: none; color: hsl(0, 0%, 52.2%); border-color: hsl(0, 0%, 81.2%);";

        protected string GetIconStyle(bool isActive) =>
            isActive
            ? "color: hsl(162, 86.6%, 32.2%); font-size: 2rem;"
            : "color: hsl(0, 0%, 52.2%); font-size: 2rem;";

        protected string GetAmountStyle(bool isActive) =>
            isActive
            ? "color: hsl(162, 86.6%, 32.2%); font-weight: 600;"
            : "color: hsl(0, 0%, 52.2%); font-weight: 600;";
    

}
}
