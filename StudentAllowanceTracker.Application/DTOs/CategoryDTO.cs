using StudentAllowanceTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.DTOs
{
    public class CategoryDTO
    {
        public Guid CategoryID { get; set; }
        public string UserID { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
        public CategoryType Type { get; set; } = CategoryType.Needs;  // enum for 50/30/20
        public decimal? BudgetAmount { get; set; }

    }
}
