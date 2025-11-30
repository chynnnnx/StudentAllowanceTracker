using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.Common.Exceptions;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand, Result<AllowanceDTO>>
    {
        private readonly IBaseRepository<Allowance> _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IHistoryService _historyService;

        public UpdateAllowanceCommandHandler(IBaseRepository<Allowance> repository,IMapper mapper, ICurrentUserService currentUser, IHistoryService historyService)  
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
            _historyService = historyService;
        }

        public async Task<Result<AllowanceDTO>> Handle(UpdateAllowanceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var allowance = await _repository.GetByIdAsync(command.AllowanceID);

            if (allowance == null || allowance.UserId != userId)
                return Result<AllowanceDTO>.Fail(ResultStatus.NotFound, "Allowance not found or access denied");

            _mapper.Map(command, allowance);
            await _repository.UpdateAsync(allowance);
            await _historyService.LogAsync(allowance, "Allowance Updated");
            var dto = _mapper.Map<AllowanceDTO>(allowance);
            return Result<AllowanceDTO>.Ok(dto);
        }
    }
}
