using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Services
{
    public class BudgetService: BaseService, IBudgetService
    {
        public BudgetService(HttpClient httpClient, ILocalStorageService localStorage)
          : base(httpClient, localStorage)
        {
        }
    }
}
