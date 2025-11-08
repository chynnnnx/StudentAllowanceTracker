using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Responses;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
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
        private readonly IBaseRepository<Allowance> _allowanceRepo;

        public CreateExpenseCommandHandler(IBaseRepository<ExpenseEntity> expenseRepo,IMapper mapper, ICurrentUserService currentUser, IBaseRepository<Allowance> allowanceRepo)
        {
            _expenseRepo = expenseRepo;
            _mapper = mapper;
            _currentUser = currentUser;
            _allowanceRepo = allowanceRepo;
        }

        public async Task <Result<ExpenseDTO>> Handle (CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<ExpenseDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var allowance = await _allowanceRepo.GetByIdAsync(command.AllowanceID);
            if (allowance == null)
                return Result<ExpenseDTO>.Fail(ResultStatus.NotFound, "Allowance not found.");

            try
            {
                allowance.Deduct(command.Amount);
            }
            catch (InvalidOperationException ex)
            {
                return Result<ExpenseDTO>.Fail(ResultStatus.ValidationError, ex.Message);
            }
            await _allowanceRepo.UpdateAsync(allowance);


            var expenses = _mapper.Map<ExpenseEntity>(command);
            expenses.ExpenseID = Guid.NewGuid();
            expenses.UserID = userId;

            await _expenseRepo.AddAsync(expenses);
            var dto = _mapper.Map<ExpenseDTO>(expenses);
            return Result<ExpenseDTO>.Ok(dto);

        }
    }
}
