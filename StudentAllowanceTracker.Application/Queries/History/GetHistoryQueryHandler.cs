using AutoMapper;
using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Queries.History
{
    public class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, Result<List<HistoryDTO>>>
    {
        private readonly IBaseRepository<HistoryEntity> _historyRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        public GetHistoryQueryHandler(IBaseRepository<HistoryEntity> historyRepo, IMapper mapper, ICurrentUserService userService)
        {
            _historyRepo = historyRepo;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<HistoryDTO>>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<List<HistoryDTO>>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var historyList = await _historyRepo.FindAsync(h => h.UserID == userId);

            if (!string.IsNullOrEmpty(request.Type))
                historyList = historyList.Where(h => h.Type == request.Type);

            historyList = historyList.OrderByDescending(h => h.Date);
            var dtoList = historyList
                .Select(h => _mapper.Map<HistoryDTO>(h))
                .ToList();

            return Result<List<HistoryDTO>>.Ok(dtoList);
        }

    }

}
