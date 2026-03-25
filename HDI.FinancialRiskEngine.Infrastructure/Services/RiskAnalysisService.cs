
using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;
using HDI.FinancialRiskEngine.Application.Exceptions;
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
            // İş konusu gerçekten var mı ve tenant altında mı kontrol edilir.
            var topic = await _context.BusinessTopics.FirstOrDefaultAsync(x => x.Id == dto.BusinessTopicId && x.TenantId == dto.TenantId && !x.IsDeleted);

            if (topic is null)
                throw new Exception("İŞ KONUSU bulunamadı.");

            // Aynı iş konusu için daha önce analiz oluşturulmuş mu kontrol edilir.
            var existingAnalysis = await _context.RiskAnalyses.AnyAsync(x => x.BusinessTopicId == dto.BusinessTopicId && x.TenantId == dto.TenantId);

            if (existingAnalysis)
                throw new BusinessException("Bu iş konusu için zaten bir risk analizi oluşturulmuş.");

            // İlgili agreement kaydı çekilir.
            var agreement = await _context.Agreements.FirstOrDefaultAsync(x => x.Id == topic.AgreementId && x.TenantId == dto.TenantId && !x.IsDeleted && x.IsActive);

            if (agreement is null)
                throw new NotFoundException("İş konusu kaydına bağlı anlaşma bulunamadı.");

            // Agreement'a bağlı aktif anahtar kelimeler çekilir.
            var keywords = await _context.AgreementKeywords.Where(x => x.AgreementId == topic.AgreementId && x.TenantId == dto.TenantId  && x.IsActive && !x.IsDeleted).ToListAsync();

            if (!keywords.Any())
            {
                throw new BusinessException("Bu anlaşma için tanımlı aktif anahtar kelime bulunamadı.");
            }

            // Basit keyword eşleşme mantığı ile toplam risk skoru hesaplanır.
            int totalScore = 0;
            List<string> matchedKeywords = new();

            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrWhiteSpace(topic.Description) &&
                    topic.Description.Contains(keyword.Keyword, StringComparison.OrdinalIgnoreCase))
                {
                    var keywordScore = (int)(keyword.RiskScore * keyword.Weight);

                    totalScore += keywordScore;
                    matchedKeywords.Add($"{keyword.Keyword} ({keywordScore})");
                }
            }

            if (totalScore == 0)
            {
                throw new BusinessException("İş konusu açıklamasında eşleşen anahtar kelime bulunamadığı için risk skoru 0 hesaplandı.");
            }

            // Toplam puana göre risk seviyesi belirlenir.
            var riskLevel = totalScore switch
            {
                <= 20 => RiskLevel.Low,
                <= 50 => RiskLevel.Medium,
                <= 100 => RiskLevel.High,
                _ => RiskLevel.Critical
            };

            // Finansal risk tutarı hesaplanır.
            decimal riskAmount = totalScore * agreement.BaseRiskRate;

            // Analiz kaydı oluşturulur.
            var analysis = new RiskAnalysis
            {
                TenantId = dto.TenantId,
                BusinessTopicId = dto.BusinessTopicId,
                MatchedKeywords = matchedKeywords.Count > 0 ? string.Join(",", matchedKeywords) : null,
                RiskScore = totalScore,
                RiskAmount = riskAmount,
                RiskLevel = riskLevel,
                AnalysisDate = DateTime.UtcNow,
                AnalysisNotes = "Otomatik analiz"
            };

            await _context.RiskAnalyses.AddAsync(analysis);

            // Ana iş konusu kaydı analiz sonucuna göre güncellenir.
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