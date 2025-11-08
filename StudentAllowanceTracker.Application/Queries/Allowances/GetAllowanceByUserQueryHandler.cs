using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentAllowanceTracker.Application.DTOs;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using AutoMapper;
using StudentAllowanceTracker.Application.Interfaces;

namespace StudentAllowanceTracker.Application.Queries.Allowances
{
    public class GetAllowanceByUserQueryHandler: IRequestHandler<GetAllowanceByUserQuery, List<AllowanceDTO>>
    {
        private readonly IBaseRepository<Allowance> _allowanceRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetAllowanceByUserQueryHandler(IBaseRepository<Allowance> allowanceRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _allowanceRepo = allowanceRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<AllowanceDTO>> Handle(GetAllowanceByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return new List<AllowanceDTO>();

            var allowances = await _allowanceRepo.FindAsync(a => a.UserId == userId);
            return _mapper.Map<List<AllowanceDTO>>(allowances);
        }
    }
}
