using MediatR;
using StudentAllowanceTracker.Application.Commands.History;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

public class DeleteHistoryCommandHandler : IRequestHandler<DeleteHistoryCommand, Result<object>>
{
    private readonly IBaseRepository<HistoryEntity> _historyRepo;
    private readonly ICurrentUserService _userService;

    public DeleteHistoryCommandHandler(IBaseRepository<HistoryEntity> historyRepo, ICurrentUserService userService)
    {
        _historyRepo = historyRepo;
        _userService = userService;
    }

    public async Task<Result<object>> Handle(DeleteHistoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<object>.Fail(ResultStatus.Unauthorized, "User not logged in.");

        var history = await _historyRepo.GetByIdAsync(request.HistoryID);

        if (history == null || history.UserID != userId)
            return Result<object>.Fail(ResultStatus.NotFound, "History not found.");

        await _historyRepo.DeleteAsync(request.HistoryID);

        return Result<object>.Ok();
    }
}
