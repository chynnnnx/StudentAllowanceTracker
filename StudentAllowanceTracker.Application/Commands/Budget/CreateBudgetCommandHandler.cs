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

namespace StudentAllowanceTracker.Application.Commands.Budget
{
    public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, Result<BudgetDTO>>
    {
        private readonly IBaseRepository<BudgetEntity> _budgetRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateBudgetCommandHandler(IBaseRepository<BudgetEntity> budgetRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _budgetRepo = budgetRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Result<BudgetDTO>> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<BudgetDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var budget = _mapper.Map<BudgetEntity>(command);
            budget.BudgetID = Guid.NewGuid();
            budget.UserID = userId;

            await _budgetRepo.AddAsync(budget);

            return Result<BudgetDTO>.Ok(_mapper.Map<BudgetDTO>(budget));
        }
    }

}
