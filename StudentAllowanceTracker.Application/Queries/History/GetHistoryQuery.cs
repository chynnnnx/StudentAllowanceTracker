using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Queries.History
{
    public class GetHistoryQuery : IRequest<Result<List<HistoryDTO>>>
    {
        public string? Type { get; set; } 
    }

}
