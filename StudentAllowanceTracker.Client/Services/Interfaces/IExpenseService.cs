 using StudentAllowanceTracker.Client.DTOs;
namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<bool> AddExpense(ExpenseDTO expenseDTO);
        Task<ExpenseDTO?> UpdateExpense(ExpenseDTO expense);
        Task<List<ExpenseDTO>?> GetExpensesByUser();
        Task DeleteExpense(Guid expenseID);
    }
}
