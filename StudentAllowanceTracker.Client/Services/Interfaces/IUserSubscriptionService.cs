using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<bool> SubscribeReminder(UserSubscriptionDTO subscriptionDTO);
        Task<bool> SendReminder();
        Task<List<UserSubscriptionDTO>> GetCurrentSubscription();
    }
}
