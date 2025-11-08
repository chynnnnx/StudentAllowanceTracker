using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using MediatR;
namespace StudentAllowanceTracker.Application.Queries.Goal
{
    public class GetGoalByUserQuery: IRequest<List<GoalsDTO>>
    {
    }
}
