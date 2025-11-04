using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;

namespace StudentAllowanceTracker.Application.Commands.Allowances
{
    public class UpdateAllowanceCommandHandler : IRequestHandler<UpdateAllowanceCommand, AllowanceDTO>
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

        public async Task<AllowanceDTO> Handle(UpdateAllowanceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var allowance = await _repository.GetByIdAsync(command.AllowanceID);

            if (allowance == null || allowance.UserId != userId)
                throw new UnauthorizedAccessException("Allowance not found or access denied.");

            allowance.Amount = command.Amount;
            allowance.Description = command.Description;
            allowance.StartDate = command.StartDate;
            allowance.EndDate = command.EndDate;
            allowance.Type = command.Type;

            await _repository.UpdateAsync(allowance);

            return _mapper.Map<AllowanceDTO>(allowance);
        }
    }
}
