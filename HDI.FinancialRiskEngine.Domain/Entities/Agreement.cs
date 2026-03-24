using HDI.FinancialRiskEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class Agreement : TenantEntity
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BaseRiskRate { get; set; }
        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; } = null!;
        public ICollection<AgreementKeyword> AgreementKeywords { get; set; } = new List<AgreementKeyword>();
        public ICollection<BusinessPartner> BusinessPartners { get; set; } = new List<BusinessPartner>();
        public ICollection<BusinessTopic> BusinessTopics { get; set; } = new List<BusinessTopic>();
    }
}
