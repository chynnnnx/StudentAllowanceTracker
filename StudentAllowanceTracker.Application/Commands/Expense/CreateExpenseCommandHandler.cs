using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using AutoMapper;
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Expense
{
    public class CreateExpenseCommandHandler: IRequestHandler<CreateExpenseCommand, Result<ExpenseDTO>>
    {
        private readonly IBaseRepository<ExpenseEntity> _expenseRepo;
        public readonly IMapper _mapper;
        public readonly ICurrentUserService _currentUser;

        public CreateExpenseCommandHandler(IBaseRepository<ExpenseEntity> expenseRepo,IMapper mapper, ICurrentUserService currentUser)
        {
            _expenseRepo = expenseRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task <Result<ExpenseDTO>> Handle (CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<ExpenseDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var expenses = _mapper.Map<ExpenseEntity>(command);
            expenses.ExpenseID = Guid.NewGuid();
            expenses.UserID = userId;

            await _expenseRepo.AddAsync(expenses);
            var dto = _mapper.Map<ExpenseDTO>(expenses);
            return Result<ExpenseDTO>.Ok(dto);

        }
    }
}
