using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class DrugImgRepository : BaseRepository<DrugImg>, IDrugImgRepository
    {
        private readonly ApplicationDbContext _context;
        public DrugImgRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
