using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Responses;
using AutoMapper;
namespace StudentAllowanceTracker.Application.Queries.Goal
{
    public class GetGoalByUserQueryHandler: IRequestHandler<GetGoalByUserQuery, List<GoalsDTO>>
    {
        private readonly IBaseRepository<GoalsEntity> _goalsRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public GetGoalByUserQueryHandler(IBaseRepository<GoalsEntity> goalsRepo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _goalsRepo = goalsRepo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<List<GoalsDTO>> Handle(GetGoalByUserQuery request, CancellationToken cancellationToken)
        {
           var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return new List<GoalsDTO>();

            var goals = await _goalsRepo.FindAsync(g => g.UserID == userId);
            return _mapper.Map<List<GoalsDTO>>(goals);

        }
    }
}
