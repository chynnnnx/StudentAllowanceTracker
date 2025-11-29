using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Dialogs.Auth
{
    public class EmailVerificationBase: LayoutComponentBase
    {
        [Inject] protected IEmailServices EmailServices { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public string Email { get; set; } = string.Empty;

        protected MudForm form;
        protected string code = string.Empty;
        protected bool isLoading = false;
        protected bool isResending = false;
        protected bool success;

        protected async Task VerifyEmail()
        {
            await form.Validate();
            if (!success) return;

            isLoading = true;
            StateHasChanged();

            var dto = new EmailVerificationDTO
            {
                Email = Email,
                Code = code
            };

            try
            {
                bool verified = await EmailServices.VerifyEmailAsync(dto);
                if (verified)
                {
                    Snackbar.Add("Email verified successfully!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add("Invalid or expired code. Please try again.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task ResendCode()
        {
            isResending = true;
            StateHasChanged();

            var dto = new EmailVerificationDTO
            {
                Email = Email,
                Code = string.Empty
            };

            try
            {
                bool sent = await EmailServices.ResendVerificationEmailAsync(dto);
                if (sent)
                {
                    Snackbar.Add("Verification code resent successfully!", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Failed to resend code. Please try again later.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
            finally
            {
                isResending = false;
                StateHasChanged();
            }
        }
    }
}
