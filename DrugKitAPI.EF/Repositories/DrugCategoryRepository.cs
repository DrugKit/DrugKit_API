using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class DrugCategoryRepository : BaseRepository<DrugCategory>, IDrugCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public DrugCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
