using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Client.DTOs
{
    public class UserSubscriptionDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool ReceiveEmail { get; set; } = true;
        public ReminderFrequency Frequency { get; set; } = ReminderFrequency.Daily;
        public DateTime LastReminderSentAt { get; set; } = DateTime.MinValue;
    }
}
