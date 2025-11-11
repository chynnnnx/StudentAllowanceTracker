using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Dialogs
{
    public class GoalsDialogBase: LayoutComponentBase
    {
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IGoalService GoalService { get; set; } = default!;

        [CascadingParameter]
        IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter]
        public GoalDTO? Goal { get; set; }

        public GoalDTO model = new();
        public DateTime? targetDate;
        public MudForm form = null!;
        public bool isValid;
        public bool isSaving;

        public bool IsEditMode => Goal != null;

        protected override void OnInitialized()
        {
            if (IsEditMode && Goal != null)
            {
                model = Goal.Clone();
            }
            else
            {
                model = new GoalDTO
                {
                    CurrentAmount = 0,
                    TargetDate = DateTime.Today.AddMonths(3)
                };
            }

            targetDate = model.TargetDate;
        }

        protected void UpdateCompletionStatus()
        {
            model.IsCompleted = decimal.Round(model.CurrentAmount, 2) >= decimal.Round(model.TargetAmount, 2);

            StateHasChanged();
        }



        protected void Cancel()
        {
            MudDialog.Cancel();
        }

        protected async Task Submit()
        {
            await form.Validate();

            if (!isValid)
            {
                Snackbar.Add("Please fill in all required fields", Severity.Warning);
                return;
            }

            if (targetDate == null)
            {
                Snackbar.Add("Please select a target date", Severity.Warning);
                return;
            }

            isSaving = true;

            try
            {
                model.TargetDate = targetDate.Value;

                bool success;
                if (IsEditMode)
                {
                    var result = await GoalService.UpdateGoal(model);
                    success = result != null;

                    if (success)
                    {
                        Snackbar.Add("Goal updated successfully!", Severity.Success);
                    }
                }
                else
                {
                    success = await GoalService.AddGoal(model);

                    if (success)
                    {
                        Snackbar.Add("Goal created successfully!", Severity.Success);
                    }
                }

                if (success)
                {
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add($"Failed to {(IsEditMode ? "update" : "create")} goal", Severity.Error);
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

    }

}
