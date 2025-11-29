using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Dialogs.Auth
{
    public class ForgotPasswordBase : LayoutComponentBase
    {
        [Inject] protected IAuthService AuthService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
        protected bool _isLoading;
        protected MudForm form;
        protected bool success;
        protected string email = string.Empty;

        protected string ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Email is required";

            if (!value.Contains("@") || !value.Contains("."))
                return "Please enter a valid email address";

            return null;
        }

        protected async Task HandleForgotPassword()
        {
            await form.Validate();
            if (!success)
                return;

            _isLoading = true;
            StateHasChanged();

            try
            {
                var result = await AuthService.ForgotPassword(email);

                if (result)
                {
                    Snackbar.Add("A 6-digit verification code has been sent to your email.", Severity.Success);
                    MudDialog.Close();

                    var parameters = new DialogParameters { { "Email", email } };
                    var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
                    DialogService.Show<ResetPassword>("", parameters, options);
                }
                else
                {
                    Snackbar.Add("Failed to send verification code. Please check your email.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        protected void HandleBackToLogin()
        {
            MudDialog.Close();
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            DialogService.Show<Login>("", parameters, options);
        }

    }
}
