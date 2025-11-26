using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Application.Commands.History;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand, Result<AllowanceDTO>>
    {
        private readonly IBaseRepository<Allowance> _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public UpdateAllowanceCommandHandler(IBaseRepository<Allowance> repository,IMapper mapper, ICurrentUserService currentUser, IMediator mediator)  
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
            _mediator = mediator;
        }

        public async Task<Result<AllowanceDTO>> Handle(UpdateAllowanceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var allowance = await _repository.GetByIdAsync(command.AllowanceID);

            if (allowance == null || allowance.UserId != userId)
                return Result<AllowanceDTO>.Fail(ResultStatus.Unauthorized, "Allowance not found or access denied.");

            _mapper.Map(command, allowance);


            await _repository.UpdateAsync(allowance);
            await HistoryHelper.LogAsync(allowance, "Allowance", _mapper, _mediator);
            var dto = _mapper.Map<AllowanceDTO>(allowance);
            return Result<AllowanceDTO>.Ok(dto);
        }
    }
}
