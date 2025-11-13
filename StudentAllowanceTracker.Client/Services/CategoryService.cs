using Blazored.LocalStorage;
using StudentAllowanceTracker.Client.Services.Interfaces;
using StudentAllowanceTracker.Client.DTOs;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Client.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(HttpClient httpClient, ILocalStorageService localStorage)
            : base(httpClient, localStorage) { }

        public async Task<bool> AddCategory(CategoryDTO categoryDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/category/create-category", categoryDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(Guid id, CategoryDTO categoryDTO)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/category/{id}", categoryDTO);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var client = await CreateAuthorizedClientAsync();
            var categories = await client.GetFromJsonAsync<List<CategoryDTO>>("api/category");
            return categories ?? new List<CategoryDTO>();
        }
        public async Task<bool> DeleteCategory(Guid id)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.DeleteAsync($"api/category/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
