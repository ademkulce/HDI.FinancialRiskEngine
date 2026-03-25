
using System.Text;
using System.Text.Json;
using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;
using Microsoft.Extensions.Options;

namespace HDI.FinancialRiskEngine.WebUI.Services
{
    public class RiskAnalysisApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public RiskAnalysisApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        /// <summary>
        /// Belirli bir business topic için risk analizini tetikler.
        /// </summary>
        public async Task<bool> CreateAsync(CreateRiskAnalysisDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}api/riskanalyses", content);

            return response.IsSuccessStatusCode;
        }
    }
}
