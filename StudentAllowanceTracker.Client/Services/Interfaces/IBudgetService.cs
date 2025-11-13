using StudentAllowanceTracker.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IBudgetService
    {

        Task<bool> AddBudget(BudgetDTO budgetDTO);
        Task<bool> UpdateBudget(Guid id, BudgetDTO budgetDTO);
        Task<List<BudgetDTO>> GetBudgetsByUser();
        Task<bool> DeleteBudget(Guid id);
    }
}
