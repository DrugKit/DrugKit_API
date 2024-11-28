using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using DrugKitAPI.EF.Data;

namespace DrugKitAPI.EF.Repositories
{
    public class UserRequestedDonationRepository : BaseRepository<UserRequestedDonation>, IUserRequestedDonationRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRequestedDonationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
