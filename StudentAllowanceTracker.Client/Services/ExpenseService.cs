using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;

namespace StudentAllowanceTracker.Client.Services
{
    public class ExpenseService : BaseService, IExpenseService
    {
        public ExpenseService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage)
        {
        }

        public async Task<bool> AddExpense(ExpenseDTO expenseDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/expense/add-expense", expenseDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<ExpenseDTO?>UpdateExpense(ExpenseDTO expense)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/expense/{expense.ExpenseID}", expense);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ExpenseDTO>()
                : null;
        }

        public async Task<List<ExpenseDTO>?> GetExpensesByUser()
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.GetAsync("api/expense");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ExpenseDTO>>();
            return null;
        }
        public async Task DeleteExpense(Guid expenseID)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.DeleteAsync($"api/expense/{expenseID}");
        }

    }
}
