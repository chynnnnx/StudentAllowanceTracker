using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;

namespace StudentAllowanceTracker.Application.Queries.History
{
    public class GetHistoryByIdQueryHandler : IRequestHandler<GetHistoryByIdQuery, Result<HistoryDTO>>
    {
        private readonly IBaseRepository<HistoryEntity> _historyRepo;
        private readonly ICurrentUserService _userService;
        private readonly IMapper _mapper;

        public GetHistoryByIdQueryHandler(IBaseRepository<HistoryEntity> historyRepo, IMapper mapper, ICurrentUserService userService)
        {
            _historyRepo = historyRepo;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<HistoryDTO>> Handle(GetHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;

            if (string.IsNullOrEmpty(userId))
                return Result<HistoryDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var record = await _historyRepo.GetByIdAsync(request.HistoryID);

            if (record == null || record.UserID != userId)
                return Result<HistoryDTO>.Fail(ResultStatus.NotFound, "History not found.");

            var dto = _mapper.Map<HistoryDTO>(record);

            return Result<HistoryDTO>.Ok(dto);
        }
    }
}
