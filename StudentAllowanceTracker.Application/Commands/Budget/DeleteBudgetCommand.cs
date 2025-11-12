using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Budget
{
    public class DeleteBudgetCommand : IRequest<object>
    {
        public Guid BudgetID { get; set; }
    }

}
