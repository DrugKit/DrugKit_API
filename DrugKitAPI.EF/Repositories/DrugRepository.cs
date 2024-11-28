using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class DrugRepository : BaseRepository<Drug>, IDrugRepository
    {
        private readonly ApplicationDbContext _context;
        public DrugRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
