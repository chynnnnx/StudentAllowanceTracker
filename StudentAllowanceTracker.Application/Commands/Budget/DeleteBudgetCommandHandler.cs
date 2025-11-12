using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
namespace StudentAllowanceTracker.Application.Commands.Budget
{
    public class DeleteBudgetCommandHandler:IRequestHandler<DeleteBudgetCommand, object>
    {
        private readonly IBaseRepository<BudgetEntity> _budgetRepo;
        public DeleteBudgetCommandHandler(IBaseRepository<BudgetEntity> budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public async Task<object> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
        {
            var budget = await _budgetRepo.GetByIdAsync(request.BudgetID);
            if (budget == null)
                return Result<object>.Fail(ResultStatus.NotFound, "Budget not found.");
            await _budgetRepo.DeleteAsync(request.BudgetID);
            return Result<object>.Ok();
        }
    }
}
