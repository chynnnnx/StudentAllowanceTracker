using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Enums;
using AutoMapper;

namespace StudentAllowanceTracker.Application.Commands.Goals
{
    public class CreateGoalCommandHandler: IRequestHandler<CreateGoalCommand, Result<GoalsDTO>>
    {
        private readonly IBaseRepository<GoalsEntity> _goalsRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IHistoryService _historyService;
        public CreateGoalCommandHandler(IBaseRepository<GoalsEntity> goalsRepo, ICurrentUserService currentUserService, IMapper mapper, IHistoryService historyService)
        {
            _goalsRepo = goalsRepo;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _historyService = historyService;
        }

        public async Task<Result<GoalsDTO>> Handle(CreateGoalCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<GoalsDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var goal = _mapper.Map<GoalsEntity>(command);

            goal.GoalID = Guid.NewGuid();
            goal.UserID = userId;
            await _goalsRepo.AddAsync(goal);
            await _historyService.LogAsync(goal, "Expense Updated");


            var dto = _mapper.Map<GoalsDTO>(goal);
            return Result<GoalsDTO>.Ok(dto);
        }
    }
}
