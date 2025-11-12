using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using MediatR;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Interfaces;
using AutoMapper;
namespace StudentAllowanceTracker.Application.Queries.Category
{
    public class GetCategoryByUserQueryHandler: IRequestHandler<GetCategoryByUserQuery, List<CategoryDTO>>
    {
        private readonly IBaseRepository<CategoryEntity> _categoryRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public GetCategoryByUserQueryHandler(IBaseRepository<CategoryEntity> categoryRepo, ICurrentUserService currentUser, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task <List<CategoryDTO>> Handle (GetCategoryByUserQuery request, CancellationToken cancellationTokean)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return new List<CategoryDTO>();
            var categories = await _categoryRepo.FindAsync(c => c.UserID == userId);
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}
