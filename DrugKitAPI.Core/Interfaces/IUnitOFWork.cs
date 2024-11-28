namespace DrugKitAPI.Core.Interfaces
{
    public interface IUnitOFWork
    {
        IDrugRepository Drug { get; }
        IMobileUserRepository MobileUser { get; }
        IAdminRepository Admin { get; }
        Task<int> CompleteAsync();
    }
}
