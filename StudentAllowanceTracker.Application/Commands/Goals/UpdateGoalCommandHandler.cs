using MediatR;

using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Application.Interfaces;
using AutoMapper;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Goals
{
    public class UpdateGoalCommandHandler: IRequestHandler<UpdateGoalCommand, Result<GoalsDTO>>
    {
        private readonly IBaseRepository<GoalsEntity> _goalsRepo;
        private readonly ICurrentUserService _currentUserService;
        public readonly IMapper _mapper;
        public UpdateGoalCommandHandler(IBaseRepository<GoalsEntity> goalsRepo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _goalsRepo = goalsRepo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GoalsDTO>> Handle(UpdateGoalCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<GoalsDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");
            var goal = await _goalsRepo.GetByIdAsync(command.GoalID);
            if (goal == null || goal.UserID != userId)
                return Result<GoalsDTO>.Fail(ResultStatus.NotFound, "Goal not found.");

            _mapper.Map(command, goal);

            goal.IsCompleted = decimal.Round(goal.CurrentAmount, 2) >= decimal.Round(goal.TargetAmount, 2);

            await _goalsRepo.UpdateAsync(goal);

            var dto = _mapper.Map<GoalsDTO>(goal);
            return Result<GoalsDTO>.Ok(dto);
        }

    }
}
