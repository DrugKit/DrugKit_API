using DrugKitAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;
        private readonly IDrugCsvService _drugCsvService;

        public DrugController(IDrugService drugService, IDrugCsvService drugCsvService)
        {
            _drugService = drugService;
            _drugCsvService = drugCsvService;
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("الملف غير صالح!");

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var drugs = await _drugCsvService.ReadAndUpdateDrugsFromCsvAsync(filePath);

            return Ok(new { message = $"تمت معالجة {drugs.Count} دواء من CSV بنجاح!" });
        }
    }
}