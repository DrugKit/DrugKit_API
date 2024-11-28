using DrugKitAPI.Core.Models;

namespace DrugKitAPI.Core.Interfaces
{
    public interface IAdminRepository : IBaseRepository<SystemAdmin>
    {
        public Task<SystemAdmin> GetByAppUserIdAsync(string id);
    }
}
