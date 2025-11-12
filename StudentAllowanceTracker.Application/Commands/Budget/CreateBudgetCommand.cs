using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Budget
{
    public class CreateBudgetCommand : IRequest<Result<BudgetDTO>>
    {
        public decimal TotalAllowance { get; set; }
        public decimal NeedsPercentage { get; set; }
        public decimal WantsPercentage { get; set; }
        public decimal SavingsPercentage { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }

}
