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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDTO>>
    {
        private readonly IBaseRepository<CategoryEntity> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateCategoryCommandHandler(IBaseRepository<CategoryEntity> categoryRepo, IMapper mapper, ICurrentUserService currentUser)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Result<CategoryDTO>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<CategoryDTO>.Fail(ResultStatus.Unauthorized, "User not logged in.");

            var category = await _categoryRepo.GetByIdAsync(command.CategoryID);
            if (category == null || category.UserID != userId)
                return Result<CategoryDTO>.Fail(ResultStatus.NotFound, "Category not found or access denied.");

            _mapper.Map(command, category);

            await _categoryRepo.UpdateAsync(category);

            return Result<CategoryDTO>.Ok(_mapper.Map<CategoryDTO>(category));
        }
    }

}
