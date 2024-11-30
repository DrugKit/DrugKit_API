using DrugKitAPI.Core.Interfaces;
using System.Text.Json;
using System.Text;

namespace DrugKitAPI.Core.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";  // ضع الـ API Key هنا

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetMedicineRecommendation(string symptoms)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "أنت شات بوت طبي تقترح الأدوية حسب الأعراض وليك انك تديله ادوية فقط في جملة لا تتعدي 20 كلمة." },
                new { role = "user", content = symptoms }
            }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                return jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            }
            else
            {
                return "حدث خطأ أثناء الحصول على التوصيات.";
            }
        }
    }
}
