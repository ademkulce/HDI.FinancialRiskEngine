using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;

namespace HDI.FinancialRiskEngine.Application.Interfaces
{
    public interface IRiskAnalysisService
    {
        Task<RiskAnalysisDto?> GetByBusinessTopicIdAsync(int businessTopicId, int tenantId);

        Task<RiskAnalysisDto> CreateAsync(CreateRiskAnalysisDto dto);

        Task<bool> DeleteAsync(int id, int tenantId);
    }
}
