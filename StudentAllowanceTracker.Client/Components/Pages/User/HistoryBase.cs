using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Shared.Helpers;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class HistoryBase : LayoutComponentBase
    {
        [Inject] protected IHistoryService HistoryService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        protected List<HistoryDTO> histories = new();
        protected List<HistoryDTO> filteredHistories = new();
        protected bool isLoading = false;
        protected string? selectedType = null;
        protected DateRange? dateRange = null;
        protected string searchString = "";

        protected List<string> historyTypes = new() { "Allowance", "Expense", "Goal", "Category" };

        protected override async Task OnInitializedAsync()
        {
            await LoadHistories();
        }

        protected async Task LoadHistories()
        {
            isLoading = true;
            try
            {
                var result = await HistoryService.GetHistories(selectedType);
                if (result != null)
                {
                    histories = result
                        .Select(h =>
                        {
                            h.Date = TimeHelper.UtcToPh(h.Date);
                            return h;
                        })
                        .OrderByDescending(h => h.Date)
                        .ToList();

                    FilterHistories();
                }
                else
                {
                    histories = new List<HistoryDTO>();
                    Snackbar.Add("Failed to load history", Severity.Warning);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading history: {ex.Message}", Severity.Error);
                histories = new List<HistoryDTO>();
            }
            finally
            {
                isLoading = false;
            }
        }


        protected void FilterHistories()
        {
            filteredHistories = histories.ToList();

            if (dateRange?.Start != null && dateRange?.End != null)
            {
                filteredHistories = filteredHistories.Where(h =>
                    h.Date.Date >= dateRange.Start.Value.Date &&
                    h.Date.Date <= dateRange.End.Value.Date).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                filteredHistories = filteredHistories.Where(h =>
                    h.Description?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true ||
                    h.Type?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }
        }

        protected async Task OnTypeFilterChanged(string? type)
        {
            selectedType = type;
            await LoadHistories();
        }

        protected void OnDateRangeChanged(DateRange? range)
        {
            dateRange = range;
            FilterHistories();
        }

        protected void OnSearchChanged(string value)
        {
            searchString = value;
            FilterHistories();
        }

        protected async Task ClearFilters()
        {
            selectedType = null;
            dateRange = null;
            searchString = "";
            await LoadHistories();
        }

        protected async Task DeleteHistory(Guid id)
        {
            bool? confirm = await DialogService.ShowMessageBox(
                "Delete History",
                "Are you sure you want to delete this history record?",
                yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    var success = await HistoryService.DeleteHistory(id);
                    if (success)
                    {
                        Snackbar.Add("History deleted successfully", Severity.Success);
                        await LoadHistories();
                    }
                    else
                    {
                        Snackbar.Add("Failed to delete history", Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error deleting history: {ex.Message}", Severity.Error);
                }
            }
        }

        protected string GetTypeColor(string? type)
        {
            return type switch
            {
                "Allowance" => "needs-color",
                "Expense" => "wants-color",
                "Goal" => "savings-color",
                "Category" => "text-warning",
                _ => "text-secondary"
            };
        }

        protected string GetTypeIcon(string? type)
        {
            return type switch
            {
                "Allowance" => Icons.Material.Filled.AccountBalanceWallet,
                "Expense" => Icons.Material.Filled.ShoppingCart,
                "Goal" => Icons.Material.Filled.TrackChanges,
                "Category" => Icons.Material.Filled.Category,
                _ => Icons.Material.Filled.History
            };
        }


        protected int GetRecordCount(string type)
        {
            return histories.Count(h => h.Type == type);
        }
    }
}