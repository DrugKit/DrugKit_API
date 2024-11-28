using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class DonationImgRepository : BaseRepository<DonationImg>, IDonationImgRepository
    {
        private readonly ApplicationDbContext _context;
        public DonationImgRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
