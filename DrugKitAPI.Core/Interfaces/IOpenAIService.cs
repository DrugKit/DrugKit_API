namespace DrugKitAPI.Core.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GetMedicineRecommendation(string symptoms);
    }
}
