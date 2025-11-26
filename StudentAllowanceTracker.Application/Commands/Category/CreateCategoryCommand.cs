using MediatR;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Enums;

namespace StudentAllowanceTracker.Application.Commands.Category
{
    public class CreateCategoryCommand: IRequest<Result<CategoryDTO>>
    {
        public string CategoryName { get; set; } = string.Empty;
        public CategoryType Type { get; set; } = CategoryType.Needs;
        public decimal? BudgetAmount { get; set; }

    }
}
