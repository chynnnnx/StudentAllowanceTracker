using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Domain.Entities;
namespace StudentAllowanceTracker.Application.Commands.Goals
{
    public class DeleteGoalCommandHandler: IRequestHandler<DeleteGoalCommand, Result<object>>
    {
        private readonly IBaseRepository<GoalsEntity> _goalsRepo;
        public DeleteGoalCommandHandler(IBaseRepository<GoalsEntity> goalsRepo)
        {
            _goalsRepo = goalsRepo;
        }

        public async Task <Result<object>> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
        {
            var goal = await _goalsRepo.GetByIdAsync(request.GoalID);
            if (goal == null)
                return Result<object>.Fail(ResultStatus.NotFound, "Goal not found.");
            await _goalsRepo.DeleteAsync(request.GoalID);
            return Result<object>.Ok();
        }
    }
}
