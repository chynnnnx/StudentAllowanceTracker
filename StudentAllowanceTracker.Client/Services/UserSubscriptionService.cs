using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
using StudentAllowanceTracker.Shared.Responses;
namespace StudentAllowanceTracker.Client.Services
{
    public class UserSubscriptionService: BaseService, IUserSubscriptionService
    {
        public UserSubscriptionService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage) { }
      

        public async Task<bool> SubscribeReminder(UserSubscriptionDTO subscriptionDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/notification/subscribe-reminder", subscriptionDTO);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> SendReminder()
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsync("api/notification/send-reminder", null);
            return response.IsSuccessStatusCode;
        }
        public async Task <List<UserSubscriptionDTO>> GetCurrentSubscription( )
        {
            var client = await CreateAuthorizedClientAsync();
            var result = await client.GetFromJsonAsync<Result<List<UserSubscriptionDTO>>>("api/notification");
            return result?.Data ?? new List<UserSubscriptionDTO>();
        }
    }
}
