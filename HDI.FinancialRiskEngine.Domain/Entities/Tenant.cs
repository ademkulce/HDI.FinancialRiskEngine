using HDI.FinancialRiskEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }

        public ICollection<Agreement> Agreements { get; set; } = new List<Agreement>();
        public ICollection<BusinessPartner> BusinessPartners { get; set; } = new List<BusinessPartner>();
    }
}
