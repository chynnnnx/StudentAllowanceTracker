using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Client.Services;
using StudentAllowanceTracker.Client.Services.Interfaces;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace StudentAllowanceTracker.Client.Services
{
    public class AllowanceService : BaseService, IAllowanceService
    {
        public AllowanceService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage)
        {
        }

        public async Task<bool> AddAllowance(AllowanceDTO allowanceDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/allowance/add-allowance", allowanceDTO);
            return response.IsSuccessStatusCode;
        }
        public async Task<AllowanceDTO?> UpdateAllowance(AllowanceDTO allowance)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/allowance/{allowance.AllowanceID}", allowance);

            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AllowanceDTO>()
                : null;
        }


        public async Task<List<AllowanceDTO>?> GetAllowanceByUser()
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.GetAsync("api/allowance");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<AllowanceDTO>>();

            return null;
        }

        public async Task  DeleteAllowance(Guid allowanceID)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.DeleteAsync($"api/allowance/{allowanceID}");
        }
    }
}
