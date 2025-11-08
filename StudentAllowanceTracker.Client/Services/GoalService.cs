using StudentAllowanceTracker.Client.Services.Interfaces;
using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.DTOs;
namespace StudentAllowanceTracker.Client.Services
{
    public class GoalService: BaseService, IGoalService
    {
        public GoalService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage)
        {
        }

        public async Task <bool> AddGoal(GoalDTO goalDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/goal/create-goal", goalDTO);
            return response.IsSuccessStatusCode;
        }
        public async Task<GoalDTO?> UpdateGoal(GoalDTO goal)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/goal/{goal.GoalID}", goal);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<GoalDTO>()
                : null;
        }
        public async Task<List<GoalDTO>?> GetGoalsByUser()
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.GetAsync("api/goal");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<GoalDTO>>();
            return null;
        }
        public async Task DeleteGoal(Guid goalID)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.DeleteAsync($"api/goal/{goalID}");
        }
    }
}
