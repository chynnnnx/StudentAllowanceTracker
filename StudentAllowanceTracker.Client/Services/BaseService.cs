using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace StudentAllowanceTracker.Client.Services
{
   
        public abstract class BaseService
        {
            private readonly HttpClient _httpClient;
            private readonly ILocalStorageService _localStorage;

            protected BaseService(HttpClient httpClient, ILocalStorageService localStorage)
            {
                _httpClient = httpClient;
                _localStorage = localStorage;
            }

            protected async Task<HttpClient> CreateAuthorizedClientAsync()
            {
                var token = await _localStorage.GetItemAsStringAsync("authToken");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = token.Trim('"');
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                return _httpClient;
            }
        }
    }

