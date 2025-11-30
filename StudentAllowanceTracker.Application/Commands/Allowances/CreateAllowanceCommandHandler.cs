using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.Commands.Allowances;
using StudentAllowanceTracker.Application.Commands.History;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs; 

public class CreateAllowanceCommandHandler : IRequestHandler<CreateAllowanceCommand, Result<AllowanceDTO>>
{
    private readonly IBaseRepository<Allowance> _allowanceRepo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
   private readonly IHistoryService _historyService;

    public CreateAllowanceCommandHandler( IBaseRepository<Allowance> allowanceRepo,  IMapper mapper, ICurrentUserService currentUser,IHistoryService historyService) 
    {
        _allowanceRepo = allowanceRepo;
        _mapper = mapper;
        _currentUser = currentUser;
        _historyService = historyService;
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
        await _historyService.LogAsync(allowance, "Allowance Created");

        var dto = _mapper.Map<AllowanceDTO>(allowance);
        return Result<AllowanceDTO>.Ok(dto);
    }

}
