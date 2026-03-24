
using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;
using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Domain.Entities;
using HDI.FinancialRiskEngine.Domain.Enums;
using HDI.FinancialRiskEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HDI.FinancialRiskEngine.Infrastructure.Services
{
    public class RiskAnalysisService : IRiskAnalysisService
    {
        private readonly AppDbContext _context;

        public RiskAnalysisService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RiskAnalysisDto?> GetByBusinessTopicIdAsync(int businessTopicId, int tenantId)
        {
            // Belirli iş konusuna ait analiz sonucunu döner.
            return await _context.RiskAnalyses
                .AsNoTracking()
                .Where(x => x.BusinessTopicId == businessTopicId && x.TenantId == tenantId)
                .Select(x => new RiskAnalysisDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    BusinessTopicId = x.BusinessTopicId,
                    MatchedKeywords = x.MatchedKeywords,
                    RiskScore = x.RiskScore,
                    RiskAmount = x.RiskAmount,
                    RiskLevel = x.RiskLevel,
                    AnalysisDate = x.AnalysisDate,
                    AnalysisNotes = x.AnalysisNotes
                }).FirstOrDefaultAsync();
        }

        public async Task<RiskAnalysisDto> CreateAsync(CreateRiskAnalysisDto dto)
        {
            // İş konusunu çekiyoruz
            var topic = await _context.BusinessTopics.FirstOrDefaultAsync(x => x.Id == dto.BusinessTopicId
                                          && x.TenantId == dto.TenantId
                                          && !x.IsDeleted);

            if (topic is null)
                throw new Exception("BusinessTopic bulunamadı.");

            // Anlaşmaya bağlı keyword’leri çekiyoruz
            var keywords = await _context.AgreementKeywords.Where(x => x.AgreementId == topic.AgreementId
                            && x.TenantId == dto.TenantId
                            && x.IsActive).ToListAsync();

            // Risk hesaplama
            int totalScore = 0;
            List<string> matchedKeywords = new();

            foreach (var keyword in keywords)
            {
                // Basit string arama (case-insensitive)
                if (topic.Description.Contains(keyword.Keyword, StringComparison.OrdinalIgnoreCase))
                {
                    totalScore += keyword.RiskScore;
                    matchedKeywords.Add(keyword.Keyword);
                }
            }

            // Risk seviyesini belirleme
            var riskLevel = totalScore switch
            {
                <= 20 => RiskLevel.Low,
                <= 50 => RiskLevel.Medium,
                <= 100 => RiskLevel.High,
                _ => RiskLevel.Critical
            };

            // Anlaşmanın base risk oranını alıyoruz
            var agreement = await _context.Agreements.FirstAsync(x => x.Id == topic.AgreementId);

            decimal riskAmount = totalScore * agreement.BaseRiskRate;

            // Analiz kaydı oluşturuluyor
            var analysis = new RiskAnalysis
            {
                TenantId = dto.TenantId,
                BusinessTopicId = dto.BusinessTopicId,
                MatchedKeywords = string.Join(",", matchedKeywords),
                RiskScore = totalScore,
                RiskAmount = riskAmount,
                RiskLevel = riskLevel,
                AnalysisDate = DateTime.UtcNow,
                AnalysisNotes = "Otomatik analiz"
            };

            await _context.RiskAnalyses.AddAsync(analysis);

            // BusinessTopic güncelleniyor
            topic.CalculatedRiskAmount = riskAmount;
            topic.Status = BusinessTopicStatus.Analyzed;

            await _context.SaveChangesAsync();

            return new RiskAnalysisDto
            {
                Id = analysis.Id,
                TenantId = analysis.TenantId,
                BusinessTopicId = analysis.BusinessTopicId,
                MatchedKeywords = analysis.MatchedKeywords,
                RiskScore = analysis.RiskScore,
                RiskAmount = analysis.RiskAmount,
                RiskLevel = analysis.RiskLevel,
                AnalysisDate = analysis.AnalysisDate,
                AnalysisNotes = analysis.AnalysisNotes
            };
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var analysis = await _context.RiskAnalyses.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId);

            if (analysis is null)
                return false;

            _context.RiskAnalyses.Remove(analysis);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}