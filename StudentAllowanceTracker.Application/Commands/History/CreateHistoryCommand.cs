using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.History
{
    public class CreateHistoryCommand : IRequest<Result<HistoryDTO>>
    {
        public string UserID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public decimal? Amount { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

    }
}
