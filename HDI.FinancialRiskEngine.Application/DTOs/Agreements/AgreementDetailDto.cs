using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Application.DTOs.Agreements
{
    public class AgreementDetailDto
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal BaseRiskRate { get; set; }

        public bool IsActive { get; set; }

        // public List<AgreementKeywordDto> Keywords { get; set; }
    }
}
