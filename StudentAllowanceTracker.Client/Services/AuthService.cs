using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            return result?.Token;
        }
        public async Task<bool> ForgotPassword(string email)
        {
            var forgotPass = new { Email = email };
            var response = await _httpClient.PatchAsJsonAsync("api/auth/password/forgot", forgotPass);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var response = await _httpClient.PatchAsJsonAsync("api/auth/password/reset", dto);
            return response.IsSuccessStatusCode;
        }

    }
}
