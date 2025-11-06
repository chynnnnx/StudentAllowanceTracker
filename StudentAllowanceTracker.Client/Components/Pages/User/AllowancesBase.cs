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

        protected decimal GetAllowanceTotal( DateTime periodStart, DateTime periodEnd, Func<DateTime, DateTime, int> getUnits = null)
        {
            decimal total = 0;

            foreach (var allowance in allowances.Where(IsActive))
            {
                if (allowance.StartDate > periodEnd) continue;

                var effectiveStart = allowance.StartDate > periodStart ? allowance.StartDate : periodStart;
                var effectiveEnd = allowance.EndDate.HasValue && allowance.EndDate.Value < periodEnd
                    ? allowance.EndDate.Value
                    : periodEnd;

                switch (allowance.Type)
                {
                    case AllowanceType.OneTime:
                        if (allowance.StartDate >= periodStart && allowance.StartDate <= periodEnd)
                            total += allowance.Amount;
                        break;

                    case AllowanceType.Daily:
                        total += allowance.Amount * ((effectiveEnd - effectiveStart).Days + 1);
                        break;

                    case AllowanceType.Weekly:
                        total += allowance.Amount * (((effectiveEnd - effectiveStart).Days + 1) / 7m);
                        break;

                    case AllowanceType.Monthly:
                        if (effectiveStart.Month == periodStart.Month && effectiveStart.Year == periodStart.Year)
                            total += allowance.Amount;
                        break;

                    case AllowanceType.Yearly:
                        var yearDays = DateTime.IsLeapYear(periodStart.Year) ? 366 : 365;
                        var daysPassed = (effectiveEnd - effectiveStart).Days + 1;
                        total += (allowance.Amount / yearDays) * daysPassed;
                        break;
                }
            }

            return total;
        }

        protected decimal GetThisMonthTotal()
        {
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var monthEnd = now;
            return GetAllowanceTotal(monthStart, monthEnd);
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
