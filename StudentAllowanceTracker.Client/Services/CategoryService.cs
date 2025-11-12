using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;

namespace StudentAllowanceTracker.Client.Services
{
    public class CategoryService:BaseService, ICategoryService

    {
        public CategoryService(HttpClient httpClient, ILocalStorageService localStorage)
          : base(httpClient, localStorage)
        {
        }


    }
}
