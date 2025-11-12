using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Queries.Budget
{
    public class GetBudgetByUserQuery: IRequest<List<BudgetDTO>>
    {
    }
}
