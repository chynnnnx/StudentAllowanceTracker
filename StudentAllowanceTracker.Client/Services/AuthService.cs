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

        public async Task<string?> RegisterAsync(RegisterDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            if (response.IsSuccessStatusCode)
                return "Registered successfully";

            var error = await response.Content.ReadAsStringAsync();
            return $"Registration failed: {error}";
        }

        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                return result?.Token;
            }

            var error = await response.Content.ReadAsStringAsync();
            return $"Login failed: {error}";
        }
        public async Task<string> ForgotPassword(string email)
        {
            var forgotPass = new { Email = email };
            var response = await _httpClient.PatchAsJsonAsync("api/auth/password/forgot", forgotPass);

            if (response.IsSuccessStatusCode)
                return "Password reset code sent successfully.";

            var error = await response.Content.ReadAsStringAsync();
            return $"Failed to send password reset code: {error}";
        }
        public async Task<string> ResetPasswordAsync(string email, string password)
        {
            var resetPass = new { Email = email, Password = password };
            var response = await _httpClient.PatchAsJsonAsync("api/auth/password/reset", resetPass);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var error = await response.Content.ReadAsStringAsync();
            return $"Failed to reset password: {error}";
        }

    }
}
