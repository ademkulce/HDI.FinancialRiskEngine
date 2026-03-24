using HDI.FinancialRiskEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Domain.Interfaces
{
    public interface IRiskCalculator
    {
        RiskAnalysis Calculate(BusinessTopic businessTopic, ICollection<AgreementKeyword> agreementKeywords, decimal baseRiskRate);
    }
}
