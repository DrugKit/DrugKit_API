using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.DTOs.Category;
using AutoMapper;

namespace DrugKitAPI.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
            => _mapper.Map<List<CategoryDTO>>(_unitOFWork.Category.GetAllAsync().Result);

        public async Task<CategoryDTO> GetCategoryAsync(int id)
        {
            if (await _unitOFWork.Category.GetByIdAsync(id) == null)
                return null!;
            return _mapper.Map<CategoryDTO>(_unitOFWork.Category.GetByIdAsync(id).Result);
        }
    }
}
