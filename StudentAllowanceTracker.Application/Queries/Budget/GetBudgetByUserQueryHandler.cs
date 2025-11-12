using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using AutoMapper;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Application.Interfaces;
using MediatR;

namespace StudentAllowanceTracker.Application.Queries.Budget
{
    public class GetBudgetByUserQueryHandler: IRequestHandler<GetBudgetByUserQuery, List<BudgetDTO>>
    {
        private readonly IBaseRepository<BudgetEntity> _budgetRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetBudgetByUserQueryHandler(IBaseRepository<BudgetEntity> budgetRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _budgetRepo = budgetRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task <List<BudgetDTO>> Handle (GetBudgetByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return new List<BudgetDTO>();
            var budgets = await _budgetRepo.FindAsync(b => b.UserID == userId);
            return _mapper.Map<List<BudgetDTO>>(budgets);
        }

    }
}
