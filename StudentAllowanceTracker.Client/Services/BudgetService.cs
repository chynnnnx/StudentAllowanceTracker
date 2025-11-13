using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
using System.Net.Http.Json;

namespace StudentAllowanceTracker.Client.Services
{
    public class BudgetService : BaseService, IBudgetService
    {
        public BudgetService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage) { }

        public async Task<bool> AddBudget(BudgetDTO budgetDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/budget/create-budget", budgetDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBudget(Guid id, BudgetDTO budgetDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/budget/{id}", budgetDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BudgetDTO>> GetBudgetsByUser()
        {
            var client = await CreateAuthorizedClientAsync();
            var budgets = await client.GetFromJsonAsync<List<BudgetDTO>>("api/budget");
            return budgets ?? new List<BudgetDTO>();
        }

        public async Task<bool> DeleteBudget(Guid id)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.DeleteAsync($"api/budget/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
