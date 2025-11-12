using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class CreateExpenseCommand: IRequest<Result<ExpenseDTO>>
    {
        public string UserID { get; set; } = string.Empty;
        public Guid AllowanceID { get; set; }
        public Guid CategoryID { get; set; } 
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
