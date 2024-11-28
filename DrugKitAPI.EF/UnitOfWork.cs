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
        public IBannedUserRepository BannedUser { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IDonationRepository Donation { get; private set; }
        public IDonationImgRepository DonationImg { get; private set; }
        public IDrugCategoryRepository DrugCategory { get; private set; }
        public IDrugImgRepository DrugImg { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IReportRepository Report { get; private set; }
        public ISideEffectRepository SideEffect { get; private set; }
        public IUserRequestedDonationRepository UserRequestedDonation { get; private set; }

        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            MobileUser = new MobileUserRepository(context);
            Admin = new AdminRepository(context);
            BannedUser = new BannedUserRepository(context);
            Category = new CategoryRepository(context);
            Donation = new DonationRepository(context);
            DonationImg = new DonationImgRepository(context);
            Drug = new DrugRepository(context);
            DrugCategory = new DrugCategoryRepository(context);
            DrugImg = new DrugImgRepository(context);
            Notification = new NotificationRepository(context);
            Report = new ReportRepository(context);
            SideEffect = new SideEffectRepository(context);
            UserRequestedDonation = new UserRequestedDonationRepository(context);
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
