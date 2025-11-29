using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Common;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Dialogs.Auth
{
    public class SignUpBase: LayoutComponentBase
    {
        [Inject] protected IAuthService AuthService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        protected RegisterDTO registerDTO = new RegisterDTO();
        protected string confirmPassword = string.Empty;
        protected MudForm form;
        protected bool success;
        protected bool _isLoading;

        protected PasswordField PasswordField = new PasswordField();
        protected PasswordField ConfirmPasswordField = new PasswordField();

        protected string ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Email is required";

            if (!value.Contains("@") || !value.Contains("."))
                return "Please enter a valid email address";

            return null;
        }

        protected string ValidateConfirmPassword(string value)
        {
            return PasswordValidator.ValidatePasswordMatch(registerDTO.Password, value);
        }

        protected async Task HandleSignUp()
        {
            await form.Validate();
            if (!success) return;

            _isLoading = true;
            StateHasChanged();

            try
            {
                var isRegistered = await AuthService.RegisterAsync(registerDTO);
                if (!isRegistered)
                {
                    Snackbar.Add("Registration failed.", Severity.Error);
                    return;
                }

                MudDialog.Close(DialogResult.Ok(registerDTO.Email));

                var parameters = new DialogParameters { ["Email"] = registerDTO.Email };
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
                DialogService.Show<EmailVerification>("Verify Your Email", parameters, options);
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



        protected void HandleLoginClick()
        {
            MudDialog.Close();
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            DialogService.Show<Login>("", parameters, options);
        }

    }
}
