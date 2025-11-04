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
        public decimal Amount { get; set; }        // Allowance amount (e.g., 500.00)
        public string? Description { get; set; }   // Optional note (e.g., "Weekly allowance from parents")

        public DateTime StartDate { get; set; }    // When this allowance starts
        public DateTime? EndDate { get; set; }     // Optional end date 

        public AllowanceType Type { get; set; }
        public AppIdentityUser User { get; set; } = default!;
    }
}
