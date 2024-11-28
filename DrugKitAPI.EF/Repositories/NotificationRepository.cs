using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
