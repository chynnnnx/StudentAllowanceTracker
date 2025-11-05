using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.DTOs
{
    public class ExpenseDTO
    {
        public Guid ExpenseID { get; set; }
        public string UserID { get; set; } = string.Empty;
        public Guid AllowanceID { get; set; }
        public string Category { get; set; } = string.Empty;  // e.g., "Food", "Entertainment"
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
