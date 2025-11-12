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

        public CreateCategoryCommandHandler(IBaseRepository<CategoryEntity> categoryRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Result<CategoryDTO>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<CategoryDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var category = _mapper.Map<CategoryEntity>(command);
            category.CategoryID = Guid.NewGuid();
            category.UserID = userId;

            await _categoryRepo.AddAsync(category);

            return Result<CategoryDTO>.Ok(_mapper.Map<CategoryDTO>(category));
        }
    }

}
