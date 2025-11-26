using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Dialogs.User;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class GoalsBase : LayoutComponentBase
    {
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IGoalService GoalService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        public List<GoalDTO> goals = new();
        public bool isLoading = false;
        public bool showCompleted = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadGoals();
        }

        protected async Task LoadGoals()
        {
            isLoading = true;
            try
            {
                var result = await GoalService.GetGoalsByUser();
                if (result != null)
                {
                    goals = result;
                }
                else
                {
                    goals = new List<GoalDTO>();
                    Snackbar.Add("Failed to load goals", Severity.Warning);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading goals: {ex.Message}", Severity.Error);
                goals = new List<GoalDTO>();
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task OpenAddDialog()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<GoalsDialog>("Create New Goal", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadGoals();
            }
        }

        protected async Task AddContribution(GoalDTO goal)
        {
            var parameters = new DialogParameters
        {
            { "Goal", goal }
        };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<GoalsDialog>("Add Contribution", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadGoals();
            }
        }

        protected async Task EditGoal(GoalDTO goal)
        {
            var parameters = new DialogParameters
        {
            { "Goal", goal }
        };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<GoalsDialog>("Edit Goal", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadGoals();
            }
        }

        protected async Task DeleteGoal(GoalDTO goal)
        {
            bool? confirm = await DialogService.ShowMessageBox(
                "Delete Goal",
                $"Are you sure you want to delete '{goal.GoalName}'?",
                yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    await GoalService.DeleteGoal(goal.GoalID);
                    Snackbar.Add("Goal deleted successfully", Severity.Success);
                    await LoadGoals();
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error deleting goal: {ex.Message}", Severity.Error);
                }
            }
        }

        protected List<GoalDTO> GetFilteredGoals()
        {
            return goals.Where(g => g.IsCompleted == showCompleted)
                       .OrderBy(g => g.TargetDate)
                       .ToList();
        }

        protected decimal GetTotalSaved()
        {
            return goals.Sum(g => g.CurrentAmount);
        }

        protected decimal GetTotalTarget()
        {
            return goals.Where(g => !g.IsCompleted).Sum(g => g.TargetAmount);
        }

        protected string GetFilterClass(bool isActive)
        {
            return isActive ? "px-4 py-2" : "px-4 py-2";
        }

        protected string GetFilterStyle(bool isActive)
        {
            return isActive
                ? "text-transform: none; color: white; background-color: var(--color-primary); border-color: var(--color-primary);"
                : "text-transform: none; color: var(--text-secondary); border-color: var(--border-light);";
        }

        protected (decimal perWeek, decimal perMonth) GetSuggestedSavings(GoalDTO goal)
        {
            var remaining = goal.TargetAmount - goal.CurrentAmount;
            if (remaining <= 0) return (0, 0);

            var daysLeft = (goal.TargetDate - DateTime.Today).Days;
            if (daysLeft <= 0) return (remaining, remaining);

            var weeks = Math.Max(1, Math.Ceiling(daysLeft / 7.0));
            var perWeek = remaining / (decimal)weeks;

            var months = Math.Max(1, Math.Ceiling(daysLeft / 30.0));
            var perMonth = remaining / (decimal)months;

            perWeek = decimal.Round(perWeek, 2);
            perMonth = decimal.Round(perMonth, 2);

            return (perWeek, perMonth);
        }
    }
}