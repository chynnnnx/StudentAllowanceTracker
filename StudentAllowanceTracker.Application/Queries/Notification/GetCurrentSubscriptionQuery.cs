using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Queries.Notification
{
    public class GetCurrentSubscriptionQuery: IRequest<Result<List<UserSubscriptionDTO>>>
    {
    }
}
