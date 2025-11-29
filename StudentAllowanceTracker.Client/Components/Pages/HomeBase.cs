using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.Components.Dialogs.Auth;
namespace StudentAllowanceTracker.Client.Components.Pages
{
    public class HomeBase: LayoutComponentBase

    {
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        [Parameter] public bool IsLoggedIn { get; set; }
        [Parameter] public string? UserName { get; set; }
       
        [Parameter] public EventCallback OnLogout { get; set; }

        protected void ShowLogin()
        {
            var options = new DialogOptions()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
            };
            DialogService.Show<Login>("Login", options);
        }

        protected async void ShowSignup()
        {
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
            };
            var dialog = DialogService.Show<SignUp>("Sign Up", options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is string email && !string.IsNullOrWhiteSpace(email))
            {
                var verifyOptions = new DialogOptions
                {
                    CloseButton = true,
                    MaxWidth = MaxWidth.ExtraSmall,
                    FullWidth = true
                };

                var parameters = new DialogParameters { ["Email"] = email };
                await DialogService.ShowAsync<EmailVerification>("Verify Your Email", parameters, verifyOptions);
            }
        }

        protected void HandleLogout() => OnLogout.InvokeAsync();

    }
}
