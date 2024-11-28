using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace DrugKitAPI.EF.Repositories
{
    public class AdminRepository:BaseRepository<SystemAdmin>,IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<SystemAdmin> GetByAppUserIdAsync(string id)
            => await _context.SystemAdmins.SingleOrDefaultAsync(t => t.ApplicationUserId == id);
    }
}
