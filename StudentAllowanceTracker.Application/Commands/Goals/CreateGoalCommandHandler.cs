using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
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
        public readonly IMapper _mapper;

        public CreateGoalCommandHandler(IBaseRepository<GoalsEntity> goalsRepo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _goalsRepo = goalsRepo;
            _currentUserService = currentUserService;
            _mapper = mapper;
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
            var dto = _mapper.Map<GoalsDTO>(goal);
            return Result<GoalsDTO>.Ok(dto);
        }
    }
}
