using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentAllowanceTracker.Domain.Entities
{
    public class BudgetEntity
    {
        public Guid BudgetID { get; set; }
        public string UserID { get; set; } = string.Empty;

        public decimal TotalAllowance { get; set; }

        // Percentages for 50/30/20 split
        public decimal NeedsPercentage { get; set; } = 50;
        public decimal WantsPercentage { get; set; } = 30;
        public decimal SavingsPercentage { get; set; } = 20;

        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime? EndDate { get; set; }

        // Computed budgets
        public decimal NeedsBudget => TotalAllowance * (NeedsPercentage / 100);
        public decimal WantsBudget => TotalAllowance * (WantsPercentage / 100);
        public decimal SavingsBudget => TotalAllowance * (SavingsPercentage / 100);

        public AppIdentityUser User { get; set; } = default!;
    }
}
