using System.Globalization;
using CsvHelper;
using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;

namespace DrugKitAPI.Core.Services
{
    public class DrugCsvService : IDrugCsvService
    {
        private readonly IUnitOFWork _unitOFWork;

        public DrugCsvService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public async Task<List<Drug>> ReadAndUpdateDrugsFromCsvAsync(string filePath)
        {
            var drugsToAdd = new List<Drug>();

            // قراءة جميع الأدوية الموجودة مسبقًا مرة واحدة فقط
            var existingDrugs = await _unitOFWork.Drug.GetAllAsync();
            var lookup = existingDrugs
                .Where(d => !string.IsNullOrWhiteSpace(d.Name) && !string.IsNullOrWhiteSpace(d.Company))
                .ToDictionary(
                    d => (d.Name.Trim().ToLower(), d.Company.Trim().ToLower()),
                    d => d
                );

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Drug>();

                foreach (var record in records)
                {
                    if (string.IsNullOrWhiteSpace(record.Name) ||
                        string.IsNullOrWhiteSpace(record.Company) ||
                        record.Price <= 0)
                        continue;

                    var key = (record.Name.Trim().ToLower(), record.Company.Trim().ToLower());

                    if (lookup.TryGetValue(key, out var existingDrug))
                    {
                        // تحديث السعر فقط
                        existingDrug.Price = record.Price;
                        _unitOFWork.Drug.Update(existingDrug);
                    }
                    else
                    {
                        drugsToAdd.Add(record);
                    }
                }
            }

            // إضافة الأدوية الجديدة دفعة واحدة
            if (drugsToAdd.Count > 0)
            {
                await _unitOFWork.Drug.AddRangeAsync(drugsToAdd);
            }

            // حفظ التغييرات دفعة واحدة
            await _unitOFWork.CompleteAsync();

            return drugsToAdd;
        }
    }
}
