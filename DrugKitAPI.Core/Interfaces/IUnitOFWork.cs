namespace DrugKitAPI.Core.Interfaces
{
    public interface IUnitOFWork
    {
        IDrugRepository Drug { get; }
        IMobileUserRepository MobileUser { get; }
        IAdminRepository Admin { get; }
        IBannedUserRepository BannedUser { get; }
        ICategoryRepository Category { get; }
        IDonationRepository Donation { get; }
        IDonationImgRepository DonationImg { get; }
        IDrugCategoryRepository DrugCategory { get; }
        IDrugImgRepository DrugImg { get; }
        INotificationRepository Notification { get; }
        IReportRepository Report { get; }
        ISideEffectRepository SideEffect { get; }
        IUserRequestedDonationRepository UserRequestedDonation { get; }
        Task<int> CompleteAsync();
    }
}
