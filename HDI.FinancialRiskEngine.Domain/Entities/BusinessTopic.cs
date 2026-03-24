using HDI.FinancialRiskEngine.Domain.Common;
using HDI.FinancialRiskEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class BusinessTopic : TenantEntity
    {
        public int AgreementId { get; set; }
        public int BusinessPartnerId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public DateTime TopicDate { get; set; }
        public BusinessTopicStatus Status { get; set; }
        public string? RawPayload { get; set; }
        public decimal? CalculatedRiskAmount { get; set; }

        public Agreement Agreement { get; set; } = null!;
        public BusinessPartner BusinessPartner { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public RiskAnalysis? RiskAnalysis { get; set; }
    }
}
