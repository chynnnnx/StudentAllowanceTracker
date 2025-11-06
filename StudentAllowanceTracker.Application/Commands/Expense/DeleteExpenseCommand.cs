using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class DeleteExpenseCommand: IRequest<Result<object>>
    {
        public Guid ExpenseID { get; set; }
    }
}
