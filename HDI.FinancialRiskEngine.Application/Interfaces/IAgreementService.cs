using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Application.Interfaces
{

    namespace HDI.FinancialRiskEngine.Application.Interfaces
    {
        public interface IAgreementService
        {
            Task<List<AgreementListDto>> GetAllAsync(int tenantId);
            Task<AgreementDetailDto?> GetByIdAsync(int id, int tenantId);
            Task<int> CreateAsync(CreateAgreementDto dto);
            Task<bool> UpdateAsync(UpdateAgreementDto dto);
            Task<bool> DeleteAsync(int id, int tenantId);
        }
    }
}
