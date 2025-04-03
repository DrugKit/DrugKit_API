using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace DrugKitAPI.Core.Services
{
    public class DrugService : IDrugService
    {
        private readonly IUnitOFWork _unitOFWork;

        public DrugService(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        //public async Task ProcessExcelFile(IFormFile file)
        //{
        //    var nonDrugKeywords = new[] { "cream", "gel", "eye contour", "lubricant", "skin care", "whitening", "soothing", "cosmetic" };
        //    var drugs = new List<Drug>();

        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);

        //        using (var package = new ExcelPackage(stream))
        //        {
        //            var worksheet = package.Workbook.Worksheets.First();
        //            var rowCount = worksheet.Dimension.Rows;

        //            drugs = Enumerable.Range(2, 20629) 
        //                .Select(row => new Drug
        //                {
        //                    name = worksheet.Cells[row, 2]?.Text.Trim().ToLower(),
        //                    arabic = worksheet.Cells[row, 3]?.Text.Trim(),
        //                    oldprice = decimal.TryParse(worksheet.Cells[row, 4]?.Text, out var oldPrice) ? oldPrice : 0,
        //                    price = decimal.TryParse(worksheet.Cells[row, 5]?.Text, out var price) ? price : 0,
        //                    active = worksheet.Cells[row, 6]?.Text.Trim().ToLower(),
        //                    company = worksheet.Cells[row, 7]?.Text.Trim(),
        //                    description = worksheet.Cells[row, 8]?.Text.Trim().ToLower(),
        //                    units = int.TryParse(worksheet.Cells[row, 9]?.Text, out var units) ? units : 0,
        //                    dosage_form = worksheet.Cells[row, 10]?.Text.Trim(),
        //                    barcode = worksheet.Cells[row, 11]?.Text.Trim(),
        //                    imported = worksheet.Cells[row, 12]?.Text.Trim()
        //                })
        //                .Where(drug => !nonDrugKeywords.Any(keyword =>
        //                    (drug.description?.Contains(keyword) ?? false) ||
        //                    (drug.active?.Contains(keyword) ?? false)))
        //                .ToList();
        //        }
        //    }

        //    await UpdateDatabase(drugs);
        //}

        //private async Task UpdateDatabase(List<Drug> drugs)
        //{
        //    var existingDrugs = await _unitOFWork.Drug.GetAllAsync();

        //    var existingDrugsDict = existingDrugs.ToDictionary(d => d.name, StringComparer.OrdinalIgnoreCase);

        //    var newDrugs = new List<Drug>();
        //    var updatedDrugs = new List<Drug>();

        //    foreach (var drug in drugs)
        //    {
        //        if (existingDrugsDict.TryGetValue(drug.name, out var existingDrug))
        //        {
        //            existingDrug.price = drug.price;
        //            existingDrug.oldprice = drug.oldprice;
        //            existingDrug.units = drug.units;
        //            existingDrug.barcode = drug.barcode;
        //            existingDrug.company = drug.company;
        //            existingDrug.description = drug.description;
        //            existingDrug.imported = drug.imported;
        //            existingDrug.dosage_form = drug.dosage_form;
        //            updatedDrugs.Add(existingDrug);
        //        }
        //        else
        //        {
        //            newDrugs.Add(drug);
        //        }
        //    }

        //    if (newDrugs.Any())
        //    {
        //        await _unitOFWork.Drug.AddRangeAsync(newDrugs);
        //    }

        //    await _unitOFWork.CompleteAsync();
        //}
    }
}
