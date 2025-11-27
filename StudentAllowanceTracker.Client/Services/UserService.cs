using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
namespace StudentAllowanceTracker.Client.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage) { }

        public async Task<List<UserDTO>?> GetUsersAsync()
        {
            {
                var client = await CreateAuthorizedClientAsync();
                var response = await client.GetAsync("api/user");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<List<UserDTO>>();

                return null;
            }

        }

        public async Task<UserDTO?> UpdateUserInfo(UserDTO userDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/user/{userDTO.Id}", userDTO);
            return response.IsSuccessStatusCode
                           ? await response.Content.ReadFromJsonAsync<UserDTO>()
                           : null;
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PatchAsJsonAsync("api/user/profile/password", changePasswordDTO);

            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? "Password changed successfully"
                : $"Failed to change password: {content}";
        }

    }
}