using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;

namespace HDI.FinancialRiskEngine.Application.Interfaces
{
    public interface IBusinessTopicService
    {
        Task<List<BusinessTopicListDto>> GetAllAsync(int tenantId);

        Task<BusinessTopicDetailDto?> GetByIdAsync(int id, int tenantId);

        Task<int> CreateAsync(CreateBusinessTopicDto dto);

        Task<bool> UpdateAsync(UpdateBusinessTopicDto dto);

        Task<bool> DeleteAsync(int id, int tenantId);
    }
}
