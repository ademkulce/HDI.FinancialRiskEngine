using HDI.FinancialRiskEngine.Domain.Common;
using HDI.FinancialRiskEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class RiskAnalysis : TenantEntity
    {
        public int BusinessTopicId { get; set; }
        public string? MatchedKeywords { get; set; }
        public int RiskScore { get; set; }
        public decimal RiskAmount { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string? AnalysisNotes { get; set; }

        public BusinessTopic BusinessTopic { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
    }
}
