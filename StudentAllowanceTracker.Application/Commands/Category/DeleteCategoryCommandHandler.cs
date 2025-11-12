using MediatR;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Enums;
namespace StudentAllowanceTracker.Application.Commands.Category
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, object>
    {
        private readonly IBaseRepository<CategoryEntity> _categoryRepo;

        public DeleteCategoryCommandHandler(IBaseRepository<CategoryEntity> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<object> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _categoryRepo.GetByIdAsync(command.CategoryID);
            if (category == null)
                return Result<object>.Fail(ResultStatus.NotFound, "Category not found.");
            await _categoryRepo.DeleteAsync(command.CategoryID);
            return Result<object>.Ok();
        }
    }

}
