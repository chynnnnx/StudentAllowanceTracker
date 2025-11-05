using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand, Result<AllowanceDTO>>
    {
        private readonly IBaseRepository<Allowance> _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateAllowanceCommandHandler(
            IBaseRepository<Allowance> repository,
            IMapper mapper,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Result<AllowanceDTO>> Handle(UpdateAllowanceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var allowance = await _repository.GetByIdAsync(command.AllowanceID);

            if (allowance == null || allowance.UserId != userId)
                return Result<AllowanceDTO>.Fail(ResultStatus.Unauthorized, "Allowance not found or access denied.");

            _mapper.Map(command, allowance);


            await _repository.UpdateAsync(allowance);

            var dto = _mapper.Map<AllowanceDTO>(allowance);
            return Result<AllowanceDTO>.Ok(dto);
        }
    }
}
