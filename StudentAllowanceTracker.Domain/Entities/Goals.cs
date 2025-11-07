using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Domain.Entities
{
    public class Goals
    {
        public Guid GoalID { get; set; }
        public string UserID { get; set; } = string.Empty;

        public string GoalName { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public AppIdentityUser User { get; set; } = default!;


    }
}
