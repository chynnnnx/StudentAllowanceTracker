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

namespace StudentAllowanceTracker.Application.Commands.Category
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDTO>>
    {
        private readonly IBaseRepository<CategoryEntity> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IBaseRepository<BudgetEntity> _budgetRepo;

        public CreateCategoryCommandHandler(
            IBaseRepository<CategoryEntity> categoryRepo,
            IMapper mapper,
            ICurrentUserService currentUser,
            IBaseRepository<BudgetEntity> budgetRepo)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _currentUser = currentUser;
            _budgetRepo = budgetRepo;
        }

        public async Task<Result<CategoryDTO>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<CategoryDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var budget = await _budgetRepo.FindOneAsync(b => b.UserID == userId);
            if (budget == null)
                return Result<CategoryDTO>.Fail(ResultStatus.NotFound, "No budget plan found for user.");

            var existingCategories = await _categoryRepo.FindAsync(c => c.UserID == userId && c.Type == command.Type);
            decimal usedBudget = existingCategories.Sum(c => c.BudgetAmount ?? 0);



            decimal typeBudget = command.Type switch
            {
                CategoryType.Needs => budget.NeedsBudget,
                CategoryType.Wants => budget.WantsBudget,
                CategoryType.Savings => budget.SavingsBudget,
                _ => 0
            };

            if ((command.BudgetAmount ?? 0) + usedBudget > typeBudget)
            {
                return Result<CategoryDTO>.Fail(ResultStatus.ValidationError,
                    $"The total budget for {command.Type} cannot exceed {typeBudget:C}. " +
                    $"Already used: {usedBudget:C}");
            }

            var category = _mapper.Map<CategoryEntity>(command);
            category.CategoryID = Guid.NewGuid();
            category.UserID = userId;

            await _categoryRepo.AddAsync(category);

            return Result<CategoryDTO>.Ok(_mapper.Map<CategoryDTO>(category));
        }
    }

}
