using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class UpdateExpenseCommand: IRequest<Result<ExpenseDTO>>
    {
        public Guid ExpenseID { get; set; }

        public Guid AllowanceID { get; set; }
        public Guid CategoryID { get; set; }
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
