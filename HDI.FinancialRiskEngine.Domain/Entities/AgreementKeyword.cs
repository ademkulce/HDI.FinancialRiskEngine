using HDI.FinancialRiskEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class AgreementKeyword : TenantEntity
    {
        public int AgreementId { get; set; }
        public string Keyword { get; set; } = null!;
        public int RiskScore { get; set; }
        public decimal Weight { get; set; }
        public bool IsActive { get; set; }

        public Agreement Agreement { get; set; } = null!;
    }
}
