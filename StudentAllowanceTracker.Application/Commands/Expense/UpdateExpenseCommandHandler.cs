using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Enums;
using AutoMapper;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Result<ExpenseDTO>>
    {
        private readonly IBaseRepository<ExpenseEntity> _expenseRepo;
        private readonly IBaseRepository<Allowance> _allowanceRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public UpdateExpenseCommandHandler(
            IBaseRepository<ExpenseEntity> expenseRepo,
            IBaseRepository<Allowance> allowanceRepo,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _expenseRepo = expenseRepo;
            _allowanceRepo = allowanceRepo;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result<ExpenseDTO>> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<ExpenseDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var expense = await _expenseRepo.GetByIdAsync(command.ExpenseID);
            if (expense == null || expense.UserID != userId)
                return Result<ExpenseDTO>.Fail(ResultStatus.NotFound, "Expense not found or access denied.");

            var allowance = await _allowanceRepo.GetByIdAsync(expense.AllowanceID);
            if (allowance == null)
                return Result<ExpenseDTO>.Fail(ResultStatus.NotFound, "Allowance not found.");

            var difference = command.Amount - expense.Amount;

            if (difference > 0)
            {
                if (difference > allowance.Amount)
                    return Result<ExpenseDTO>.Fail(ResultStatus.ValidationError, "Not enough allowance to cover the increased expense.");

                allowance.Deduct(difference);
            }
            else if (difference < 0)
            {
                allowance.Amount += Math.Abs(difference);
            }

            await _allowanceRepo.UpdateAsync(allowance);

            _mapper.Map(command, expense);
            await _expenseRepo.UpdateAsync(expense);

            return Result<ExpenseDTO>.Ok(_mapper.Map<ExpenseDTO>(expense));
        }

    }
}
