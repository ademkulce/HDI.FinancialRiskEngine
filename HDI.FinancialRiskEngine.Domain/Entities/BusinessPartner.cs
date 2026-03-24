using HDI.FinancialRiskEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class BusinessPartner : TenantEntity
    {
        public int AgreementId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string ApiKey { get; set; } = null!;
        public bool IsActive { get; set; }

        public Agreement Agreement { get; set; } = null!;
        public Tenant Tenant { get; set; } = null!;
        public ICollection<BusinessTopic> BusinessTopics { get; set; } = new List<BusinessTopic>();
    }
}
