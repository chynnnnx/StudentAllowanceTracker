using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;
namespace StudentAllowanceTracker.Client.Services
{
    public class EmailServices: IEmailServices
    {
        private readonly HttpClient _httpClient;

        public EmailServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyEmailAsync(EmailVerificationDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/email/verify-email", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResendVerificationEmailAsync(EmailVerificationDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/email/resend-verification", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
