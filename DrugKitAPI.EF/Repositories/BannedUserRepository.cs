using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class BannedUserRepository : BaseRepository<BannedUser>, IBannedUserRepository
    {
        private readonly ApplicationDbContext _context;
        public BannedUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}