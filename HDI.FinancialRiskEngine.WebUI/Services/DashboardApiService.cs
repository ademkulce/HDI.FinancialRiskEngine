using System.Text.Json;
using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;
using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;
using HDI.FinancialRiskEngine.WebUI.ViewModels;
using Microsoft.Extensions.Options;

namespace HDI.FinancialRiskEngine.WebUI.Services
{
    public class DashboardApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public DashboardApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(int tenantId)
        {
            // API'den gerekli verileri çekip dashboard modeline dönüştürür.
            var dashboard = new DashboardViewModel();

            var agreementResponse = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/agreements/{tenantId}");
            var partnerResponse = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/businesspartners/{tenantId}");
            var topicResponse = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}api/businesstopics/{tenantId}");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (agreementResponse.IsSuccessStatusCode)
            {
                var agreementJson = await agreementResponse.Content.ReadAsStringAsync();
                var agreements = JsonSerializer.Deserialize<List<AgreementListDto>>(agreementJson, jsonOptions) ?? new List<AgreementListDto>();
                dashboard.TotalAgreements = agreements.Count;
            }

            if (partnerResponse.IsSuccessStatusCode)
            {
                var partnerJson = await partnerResponse.Content.ReadAsStringAsync();
                var partners = JsonSerializer.Deserialize<List<BusinessPartnerListDto>>(partnerJson, jsonOptions) ?? new List<BusinessPartnerListDto>();
                dashboard.TotalBusinessPartners = partners.Count;
            }

            if (topicResponse.IsSuccessStatusCode)
            {
                var topicJson = await topicResponse.Content.ReadAsStringAsync();
                var topics = JsonSerializer.Deserialize<List<BusinessTopicListDto>>(topicJson, jsonOptions) ?? new List<BusinessTopicListDto>();

                dashboard.TotalBusinessTopics = topics.Count;
                dashboard.TotalAnalyzedTopics = topics.Count(x => x.Status.ToString() == "Analyzed");

                // Son 5 iş konusunu dashboard'da göstermek için hazırlıyoruz.
                dashboard.RecentBusinessTopics = topics
                    .OrderByDescending(x => x.TopicDate)
                    .Take(5)
                    .Select(x => new RecentBusinessTopicViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        ReferenceNumber = x.ReferenceNumber,
                        TopicDate = x.TopicDate,
                        Status = x.Status.ToString(),
                        CalculatedRiskAmount = x.CalculatedRiskAmount
                    }).ToList();
                             
            }

            return dashboard;
        }
    }
}