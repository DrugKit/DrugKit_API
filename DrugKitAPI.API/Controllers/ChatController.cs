using DrugKitAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public ChatController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartChat([FromBody] string symptoms)
        {
            var response = await _openAIService.GetMedicineRecommendation(symptoms);

            return Ok(new
            {
                message = response,
                advice = "تأكد من استشارة طبيب قبل استخدام أي دواء."
            });
        }

        [HttpPost("validate")]
        public IActionResult ValidateInput([FromBody] string input)
        {
            if (string.IsNullOrEmpty(input) || !IsValidSymptoms(input))
            {
                return BadRequest("حاول مرة أخرى");
            }

            return Ok("المدخلات صحيحة");
        }

        private bool IsValidSymptoms(string input)
        {
            var validSymptoms = new[] { "صداع", "سخونة", "ألم" };
            foreach (var symptom in validSymptoms)
            {
                if (input.Contains(symptom))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
