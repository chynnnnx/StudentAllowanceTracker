using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAllowanceTracker.Application.Commands.Allowances;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

public class CreateAllowanceCommandHandler : IRequestHandler<CreateAllowanceCommand, Result<AllowanceDTO>>
{
    private readonly IBaseRepository<Allowance> _allowanceRepo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CreateAllowanceCommandHandler(
        IBaseRepository<Allowance> allowanceRepo,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _allowanceRepo = allowanceRepo;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Result<AllowanceDTO>> Handle(CreateAllowanceCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<AllowanceDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

        var allowance = _mapper.Map<Allowance>(command);
        allowance.AllowanceID = Guid.NewGuid();
        allowance.UserId = userId; 

        await _allowanceRepo.AddAsync(allowance);

        var dto = _mapper.Map<AllowanceDTO>(allowance);
        return Result<AllowanceDTO>.Ok(dto);
    }
}
