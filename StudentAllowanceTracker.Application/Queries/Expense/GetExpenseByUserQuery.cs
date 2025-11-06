using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Queries.Expense
{
    public class GetExpenseByUserQuery: IRequest<List<ExpenseDTO>>
    {
    }
}
