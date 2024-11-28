using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class DonationRepository : BaseRepository<Donation>, IDonationRepository
    {
        private readonly ApplicationDbContext _context;
        public DonationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
