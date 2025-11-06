using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Domain.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using AutoMapper;
using StudentAllowanceTracker.Application.Interfaces;

namespace StudentAllowanceTracker.Application.Queries.Expense
{
    public class GetExpenseByUserQueryHandler: IRequestHandler<GetExpenseByUserQuery, List<ExpenseDTO>>
    {
        private readonly IBaseRepository<ExpenseEntity> _expenseRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetExpenseByUserQueryHandler(IBaseRepository<ExpenseEntity> expenseRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _expenseRepo = expenseRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<ExpenseDTO>> Handle (GetExpenseByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return new List<ExpenseDTO>();
            var expenses = await _expenseRepo.FindAsync(e => e.UserID == userId);
            return _mapper.Map<List<ExpenseDTO>>(expenses);
        }
    }
}
