using StudentAllowanceTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Domain.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool ReceiveEmail { get; set; } = true;
        public ReminderFrequency Frequency { get; set; } = ReminderFrequency.Daily;
        public DateTime LastReminderSentAt { get; set; } = DateTime.MinValue;

        public AppIdentityUser User { get; set; } = null!;

    }
}
