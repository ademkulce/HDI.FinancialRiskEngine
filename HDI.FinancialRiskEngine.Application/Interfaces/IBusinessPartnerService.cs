using System;
using System.Collections.Generic;
using System.Text;

using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;

namespace HDI.FinancialRiskEngine.Application.Interfaces
{
    public interface IBusinessPartnerService
    {
        Task<List<BusinessPartnerListDto>> GetAllAsync(int tenantId);

        Task<BusinessPartnerDetailDto?> GetByIdAsync(int id, int tenantId);

        Task<int> CreateAsync(CreateBusinessPartnerDto dto);

        Task<bool> UpdateAsync(UpdateBusinessPartnerDto dto);

        Task<bool> DeleteAsync(int id, int tenantId);

        Task<string> RegenerateApiKeyAsync(int id, int tenantId);
    }
}
