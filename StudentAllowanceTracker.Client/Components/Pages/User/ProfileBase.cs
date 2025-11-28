using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Components.Pages.User
{
    public class ProfileBase : ComponentBase
    {
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IUserService UserService { get; set; } = default!;

        protected UserDTO userInfo = new();
        protected UserDTO originalUserInfo = new();
        protected ChangePasswordDTO passwordChange = new();
        protected bool isLoading = true;
        protected bool isEditingProfile = false;
        protected bool isChangingPassword = false;
        protected bool showPassword = false;
        protected bool showNewPassword = false;
        protected bool showConfirmPassword = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserInfoAsync();
        }

        protected async Task LoadUserInfoAsync()
        {
            isLoading = true;
            try
            {
                var result = await UserService.GetUserInfoAsync();
                if (result != null)
                {
                    userInfo = result;
                    originalUserInfo = new UserDTO
                    {
                        Id = result.Id,
                        Email = result.Email,
                        FirstName = result.FirstName,
                        LastName = result.LastName
                    };
                }
                else
                {
                    Snackbar.Add("Failed to load profile", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task UpdateProfile()
        {
            try
            {
                var result = await UserService.UpdateUserInfo(userInfo);
                if (result != null)
                {
                    Snackbar.Add("Profile updated successfully!", Severity.Success);
                    isEditingProfile = false;
                    originalUserInfo = new UserDTO
                    {
                        Id = result.Id,
                        Email = result.Email,
                        FirstName = result.FirstName,
                        LastName = result.LastName
                    };
                }
                else
                {
                    Snackbar.Add("Failed to update profile", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile: {ex.Message}", Severity.Error);
            }
        }

        protected async Task ChangePassword()
        {
            if (passwordChange.NewPassword != passwordChange.ConfirmPassword)
            {
                Snackbar.Add("Passwords do not match", Severity.Warning);
                return;
            }

            try
            {
                var result = await UserService.ChangePasswordAsync(passwordChange);
                if (result.Contains("successfully"))
                {
                    Snackbar.Add("Password changed successfully!", Severity.Success);
                    CancelPasswordChange();
                }
                else
                {
                    Snackbar.Add(result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error changing password: {ex.Message}", Severity.Error);
            }
        }

        protected void CancelEdit()
        {
            isEditingProfile = false;
            userInfo.FirstName = originalUserInfo.FirstName;
            userInfo.LastName = originalUserInfo.LastName;
            userInfo.Email = originalUserInfo.Email;
        }

        protected void CancelPasswordChange()
        {
            isChangingPassword = false;
            passwordChange = new();
            showPassword = false;
            showNewPassword = false;
            showConfirmPassword = false;
        }

        protected bool IsProfileValid() =>
            !string.IsNullOrWhiteSpace(userInfo.FirstName) &&
            !string.IsNullOrWhiteSpace(userInfo.LastName) &&
            !string.IsNullOrWhiteSpace(userInfo.Email);

        protected bool IsPasswordValid() =>
            !string.IsNullOrWhiteSpace(passwordChange.CurrentPassword) &&
            !string.IsNullOrWhiteSpace(passwordChange.NewPassword) &&
            passwordChange.NewPassword.Length >= 6 &&
            passwordChange.NewPassword == passwordChange.ConfirmPassword;
    }
}
