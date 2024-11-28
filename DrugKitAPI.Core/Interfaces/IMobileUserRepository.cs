using DrugKitAPI.Core.Models;

namespace DrugKitAPI.Core.Interfaces
{
    public interface IMobileUserRepository : IBaseRepository<MobileUser>
    {
        public Task<MobileUser> GetByAppUserIdAsync(string id);
    }
}