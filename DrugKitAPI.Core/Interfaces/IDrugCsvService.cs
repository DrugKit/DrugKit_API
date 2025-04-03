using DrugKitAPI.Core.Models;

namespace DrugKitAPI.Core.Interfaces
{
    public interface IDrugCsvService
    {
        Task<List<Drug>> ReadAndUpdateDrugsFromCsvAsync(string filePath);
    }
}
