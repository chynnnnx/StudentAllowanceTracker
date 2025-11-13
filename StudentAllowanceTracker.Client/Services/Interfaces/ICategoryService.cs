using StudentAllowanceTracker.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Client.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(CategoryDTO categoryDTO);
        Task<bool> UpdateCategory(Guid id, CategoryDTO categoryDTO);
        Task<List<CategoryDTO>> GetAllCategories();
        Task<bool> DeleteCategory(Guid id);
    }
}
