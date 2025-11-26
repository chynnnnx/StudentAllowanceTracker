using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
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
        private readonly IMediator _mediator;
        public UpdateExpenseCommandHandler( IBaseRepository<ExpenseEntity> expenseRepo,IBaseRepository<Allowance> allowanceRepo, ICurrentUserService currentUser, IMapper mapper,
            IMediator mediator)
        {
            _expenseRepo = expenseRepo;
            _allowanceRepo = allowanceRepo;
            _currentUser = currentUser;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<ExpenseDTO>> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<ExpenseDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var expense = await _expenseRepo.GetByIdAsync(command.ExpenseID);
            if (expense == null || expense.UserID != userId)
                return Result<ExpenseDTO>.Fail(ResultStatus.NotFound, "Expense not found or access denied.");

            _mapper.Map(command, expense);
            await _expenseRepo.UpdateAsync(expense);
            await HistoryHelper.LogAsync(expense, "Expense", _mapper, _mediator);

            return Result<ExpenseDTO>.Ok(_mapper.Map<ExpenseDTO>(expense));
        }

    }
}
