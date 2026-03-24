using HDI.FinancialRiskEngine.Domain.Enums;

namespace HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses
{
    public class RiskAnalysisDto
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int BusinessTopicId { get; set; }

        public string? MatchedKeywords { get; set; }

        public int RiskScore { get; set; }

        public decimal RiskAmount { get; set; }

        public RiskLevel RiskLevel { get; set; }

        public DateTime AnalysisDate { get; set; }

        public string? AnalysisNotes { get; set; }
    }
}
