using System.Text;
using System.Text.Json;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;
using Microsoft.Extensions.Options;

namespace HDI.FinancialRiskEngine.WebUI.Services
{
    public class BusinessPartnerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public BusinessPartnerApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        /// <summary>
        /// Tüm business partner kayıtlarını getirir
        /// </summary>
        public async Task<List<BusinessPartnerListDto>> GetAllAsync(int tenantId)
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/businesspartners/{tenantId}");

            if (!response.IsSuccessStatusCode)
                return new List<BusinessPartnerListDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<BusinessPartnerListDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BusinessPartnerListDto>();
        }

        /// <summary>
        /// Yeni business partner oluşturur
        /// </summary>
        public async Task<bool> CreateAsync(CreateBusinessPartnerDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}api/businesspartners", content);

            return response.IsSuccessStatusCode;
        }
    }
}
