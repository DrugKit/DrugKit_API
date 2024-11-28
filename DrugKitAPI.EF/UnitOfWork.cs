using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.EF.Data;
using DrugKitAPI.EF.Repositories;

namespace DrugKitAPI.EF
{
    public class UnitOfWork:IUnitOFWork
    {
        public IDrugRepository Drug {  get; private set; }
        public IMobileUserRepository MobileUser { get; private set; }
        public IAdminRepository Admin { get; private set; }

        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            MobileUser = new MobileUserRepository(context);
            Admin = new AdminRepository(context);
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
