using System;
using System.Collections.Generic;
using System.Text;

using HDI.FinancialRiskEngine.Domain.Enums;

namespace HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics
{
    public class BusinessTopicDetailDto
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int AgreementId { get; set; }

        public int BusinessPartnerId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ReferenceNumber { get; set; }

        public DateTime TopicDate { get; set; }

        public BusinessTopicStatus Status { get; set; }

        public string? RawPayload { get; set; }

        public decimal? CalculatedRiskAmount { get; set; }

        // public RiskAnalysisDto RiskAnalysis { get; set; }
    }
}
