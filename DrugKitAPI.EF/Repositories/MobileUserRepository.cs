using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace DrugKitAPI.EF.Repositories
{
    public class MobileUserRepository:BaseRepository<MobileUser>,IMobileUserRepository
    {
        private readonly ApplicationDbContext _context;
        public MobileUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<MobileUser> GetByAppUserIdAsync(string id)
            => await _context.MobileUsers.SingleOrDefaultAsync(t => t.ApplicationUserId == id);
    }
}
