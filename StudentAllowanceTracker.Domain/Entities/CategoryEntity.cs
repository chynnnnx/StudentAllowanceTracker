using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Domain.Entities
{
    public class CategoryEntity
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;  
        public decimal BudgetAmount { get; set; }          
        public string UserID { get; set; } = string.Empty; 

        public AppIdentityUser User { get; set; } = default!;
        public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();
    }
}
