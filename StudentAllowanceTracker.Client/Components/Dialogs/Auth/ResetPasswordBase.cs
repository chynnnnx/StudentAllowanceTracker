using Microsoft.AspNetCore.Components;
using StudentAllowanceTracker.Client.Services.Interfaces;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Common;
using StudentAllowanceTracker.Client.DTOs;


namespace StudentAllowanceTracker.Client.Components.Dialogs.Auth
{
    public class ResetPasswordBase: LayoutComponentBase
    {
        [Inject] protected IAuthService AuthService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
        [Parameter] public string Email { get; set; } = string.Empty;
        protected PasswordField NewPasswordField = new PasswordField();
        protected PasswordField ConfirmPasswordField = new PasswordField();


        protected bool _isLoading;
        protected MudForm form;
        protected ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO();
        protected bool success;

        protected override void OnInitialized()
        {
            resetPasswordDTO.Email = Email;
        }
        protected string ValidateConfirmPassword(string value)
        {
            return PasswordValidator.ValidatePasswordMatch(resetPasswordDTO.NewPassword, value);
        }

        protected async Task HandleResetPassword()
        {
            await form.Validate();
            if (!success)
                return;

            _isLoading = true;
            StateHasChanged();

            try
            {
                var result = await AuthService.ResetPasswordAsync(resetPasswordDTO);

                if (result)
                {
                    Snackbar.Add("Password reset successfully! Please login with your new password.", Severity.Success);
                    MudDialog.Close();

                    var parameters = new DialogParameters();
                    var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
                    DialogService.Show<Login>("", parameters, options);
                }
                else
                {
                    Snackbar.Add("Failed to reset password. Please try again.", Severity.Error);
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
