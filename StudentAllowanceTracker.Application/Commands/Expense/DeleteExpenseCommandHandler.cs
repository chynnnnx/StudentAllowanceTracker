using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class DeleteExpenseCommandHandler: IRequestHandler<DeleteExpenseCommand, Result<object>>
    {
        private readonly IBaseRepository<ExpenseEntity> _expenseRepo;
        public DeleteExpenseCommandHandler(IBaseRepository<ExpenseEntity> expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        public async Task<Result<object>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepo.GetByIdAsync(request.ExpenseID);
            if (expense == null)
                return Result<object>.Fail(ResultStatus.NotFound, "Expense not found.");
            await _expenseRepo.DeleteAsync(request.ExpenseID);
            return Result<object>.Ok();
        }
    }
}
