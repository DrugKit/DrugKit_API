using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class SideEffectRepository : BaseRepository<SideEffect>, ISideEffectRepository
    {
        private readonly ApplicationDbContext _context;
        public SideEffectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
