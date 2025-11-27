using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>?> GetUsersAsync();
        Task<UserDTO?> UpdateUserInfo(UserDTO userDTO);
        Task<string> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
    }
}
