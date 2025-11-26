using StudentAllowanceTracker.Client.Services.Interfaces;
using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.DTOs;
using System.Net.Http.Json;

namespace StudentAllowanceTracker.Client.Services
{
    public class HistoryService : BaseService, IHistoryService
    {
        public HistoryService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage)
        {
        }

        public async Task<bool> AddHistory(HistoryDTO historyDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/history", historyDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<HistoryDTO>?> GetHistories(string? type = null)
        {
            var client = await CreateAuthorizedClientAsync();
            var url = string.IsNullOrEmpty(type) ? "api/history" : $"api/history?type={type}";
            return await client.GetFromJsonAsync<List<HistoryDTO>>(url);
        }

        public async Task<HistoryDTO?> GetHistoryById(Guid id)
        {
            var client = await CreateAuthorizedClientAsync();
            return await client.GetFromJsonAsync<HistoryDTO>($"api/history/{id}");
        }

        public async Task<bool> DeleteHistory(Guid id)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.DeleteAsync($"api/history/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
