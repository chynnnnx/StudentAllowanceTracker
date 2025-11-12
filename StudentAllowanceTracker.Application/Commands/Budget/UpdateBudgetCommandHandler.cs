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
    public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Result<BudgetDTO>>
    {
        private readonly IBaseRepository<BudgetEntity> _budgetRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateBudgetCommandHandler(IBaseRepository<BudgetEntity> budgetRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _budgetRepo = budgetRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Result<BudgetDTO>> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<BudgetDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var budget = await _budgetRepo.GetByIdAsync(command.BudgetID);
            if (budget == null || budget.UserID != userId)
                return Result<BudgetDTO>.Fail(ResultStatus.NotFound, "Budget not found or access denied.");

            _mapper.Map(command, budget);

            await _budgetRepo.UpdateAsync(budget);

            return Result<BudgetDTO>.Ok(_mapper.Map<BudgetDTO>(budget));
        }
    }

}
