using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Notification
{
    public class SubscribeReminderCommand : IRequest<Result<UserSubscriptionDTO>>
    {
        public bool ReceiveEmail { get; set; }
        public ReminderFrequency Frequency { get; set; }
    }
}