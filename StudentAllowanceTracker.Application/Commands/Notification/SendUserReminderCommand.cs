using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Commands.Notification
{
    public class SendUserReminderCommand: IRequest <Result<List<UserSubscriptionDTO>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
