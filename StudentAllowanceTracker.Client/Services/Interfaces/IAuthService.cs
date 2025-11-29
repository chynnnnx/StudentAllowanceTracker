using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<string?> LoginAsync(LoginDTO dto);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
    }
}
