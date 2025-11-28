using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Domain.Entities
{
    public class AppIdentityUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public ICollection<Allowance> Allowances { get; set; } = new List<Allowance>();
        public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();
        public ICollection<GoalsEntity> Goals { get; set; } = new List<GoalsEntity>();
        public ICollection<HistoryEntity> Histories { get; set; } = new List<HistoryEntity>();
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();



    }
}
