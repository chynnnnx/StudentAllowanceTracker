using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Services.Interfaces;
using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Security;
using StudentAllowanceTracker.Client.Components.Common;
using StudentAllowanceTracker.Client.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace StudentAllowanceTracker.Client.Components.Dialogs.Auth
{
    public class LoginBase: LayoutComponentBase
    {
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected IAuthService AuthService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ILocalStorageService _localStorage { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;


        [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
        protected bool _isLoading;
        protected MudForm form;
        protected bool success;
        protected LoginDTO loginDTO = new LoginDTO();
        protected PasswordField LoginPasswordField = new PasswordField();


        protected async Task HandleLogin()
        {
            await form.Validate();
            if (!success)
                return;

            _isLoading = true;
            StateHasChanged();

            try
            {
                var token = await AuthService.LoginAsync(loginDTO);

                if (token == null)
                {
                    Snackbar.Add("Invalid email or password.", Severity.Error);
                    return;
                }

                await _localStorage.SetItemAsync("authToken", token);

                var authStateProvider = (CustomAuthStateProvider)AuthStateProvider;
                authStateProvider.NotifyUserAuthentication(token);

                MudDialog.Close(DialogResult.Ok(true));
                Navigation.NavigateTo("/dashboard", true);
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

        protected void HandleForgotPasswordClick()
        {
            MudDialog.Close();
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            DialogService.Show<ForgotPassword>("", parameters, options);
        }

        protected void HandleSignUpClick()
        {
            MudDialog.Close();
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            DialogService.Show<SignUp>("", parameters, options);
        }

        protected void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
