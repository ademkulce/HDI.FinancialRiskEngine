
using System.Text;
using System.Text.Json;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;
using Microsoft.Extensions.Options;

namespace HDI.FinancialRiskEngine.WebUI.Services
{
    public class BusinessTopicApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public BusinessTopicApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        /// <summary>
        /// Belirli tenant'a ait iş konularını getirir.
        /// </summary>
        public async Task<List<BusinessTopicListDto>> GetAllAsync(int tenantId)
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/businesstopics/{tenantId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<BusinessTopicListDto>();
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<BusinessTopicListDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BusinessTopicListDto>();
        }

        /// <summary>
        /// Yeni business topic kaydı oluşturur.
        /// </summary>
        public async Task<bool> CreateAsync(CreateBusinessTopicDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}api/businesstopics", content);

            return response.IsSuccessStatusCode;
        }
    }
}