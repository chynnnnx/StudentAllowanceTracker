using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Enums;
using AutoMapper;
namespace StudentAllowanceTracker.Application.Commands.History
{
    public class CreateHistoryCommandHandler: IRequestHandler<CreateHistoryCommand, Result<HistoryDTO>>
    {
        private readonly IBaseRepository<HistoryEntity> _historyRepo;
        private readonly ICurrentUserService _userService;
        private readonly IMapper _mapper;

        public CreateHistoryCommandHandler(IBaseRepository<HistoryEntity> historyRepo,  IMapper mapper, ICurrentUserService userService)
        {
            _historyRepo = historyRepo;
            _mapper = mapper;
            _userService = userService;

        }

        public async Task<Result<HistoryDTO>> Handle(CreateHistoryCommand command, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<HistoryDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var history = _mapper.Map<HistoryEntity>(command);
            history.HistoryID = Guid.NewGuid();
            history.UserID = userId;
            await _historyRepo.AddAsync(history);
            var dto = _mapper.Map<HistoryDTO>(history);
            return Result<HistoryDTO>.Ok(dto);
        }
    }
}
