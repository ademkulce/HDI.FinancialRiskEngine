using System.Text;
using System.Text.Json;
using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using Microsoft.Extensions.Options;

namespace HDI.FinancialRiskEngine.WebUI.Services
{
    public class AgreementApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public AgreementApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        public async Task<List<AgreementListDto>> GetAllAsync(int tenantId)
        {
            // Agreement listesini WebApi üzerinden çeker.
            var response = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/agreements/{tenantId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<AgreementListDto>();
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<AgreementListDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<AgreementListDto>();
        }

        public async Task<bool> CreateAsync(CreateAgreementDto dto)
        {
            // UI üzerinden girilen veriler api ye gönderilir. 
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiSettings.BaseUrl}api/agreements", content);

            return response.IsSuccessStatusCode;
        }
    }
}