using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners
{
    public class BusinessPartnerDetailDto
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int AgreementId { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string ApiKey { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
