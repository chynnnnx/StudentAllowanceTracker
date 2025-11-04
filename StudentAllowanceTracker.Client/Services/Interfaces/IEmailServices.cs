using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> VerifyEmailAsync(EmailVerificationDTO dto);
        Task<bool> ResendVerificationEmailAsync(EmailVerificationDTO dto);
    }
}
