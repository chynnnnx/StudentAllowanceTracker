using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Shared.Enums;
namespace StudentAllowanceTracker.Domain.Entities
{
    public class Allowance
    {
        public Guid AllowanceID { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }      
        public string? Description { get; set; }  

        public DateTime StartDate { get; set; }  
        public DateTime? EndDate { get; set; }  

        public AllowanceType Type { get; set; }
        public AppIdentityUser User { get; set; } = default!;

        public ICollection<ExpenseEntity> Expenses { get; set; } = new List <ExpenseEntity>();
     
    }
}
