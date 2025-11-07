using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Commands.Goals
{
    public class CreateGoalCommand: IRequest<Result<GoalsDTO>>
    {
        public string GoalName { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public string? Description { get; set; }
    }
}
