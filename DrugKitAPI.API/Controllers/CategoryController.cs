using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories() 
        {
            var result=await _categoryService.GetAllCategoriesAsync();
            return result != null ? Ok(result) : NotFound();
        }
    }
}
