using DrugKitAPI.Core.DTOs.Category;
using DrugKitAPI.Core.Models;

namespace DrugKitAPI.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetCategoryAsync(int id);
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
    }
}
