using MediatR;
using StudentAllowanceTracker.Application.DTOs;
using StudentAllowanceTracker.Shared.Enums;
using StudentAllowanceTracker.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Commands.Category
{

    public class UpdateCategoryCommand : IRequest<Result<CategoryDTO>>
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public CategoryType Type { get; set; } = CategoryType.Needs;
        public decimal? BudgetAmount { get; set; }
    }
}
