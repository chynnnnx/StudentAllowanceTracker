using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Shared.Responses;
namespace StudentAllowanceTracker.Application.Commands.Goals
{
    public class DeleteGoalCommand: IRequest<Result<object>>
    {
        public Guid GoalID { get; set; }
    }
    
    }

