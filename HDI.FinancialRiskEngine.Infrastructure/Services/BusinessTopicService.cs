using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;
using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Domain.Entities;
using HDI.FinancialRiskEngine.Domain.Enums;
using HDI.FinancialRiskEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HDI.FinancialRiskEngine.Infrastructure.Services
{
    public class BusinessTopicService : IBusinessTopicService
    {
        private readonly AppDbContext _context;

        public BusinessTopicService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusinessTopicListDto>> GetAllAsync(int tenantId)
        {
            // Belirli tenant'a ait, silinmemiş iş konularını listeler.
            // Liste ekranı için sadece gerekli alanlar DTO'ya projekte edilir.
            return await _context.BusinessTopics
                .AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted)
                .OrderByDescending(x => x.TopicDate)
                .Select(x => new BusinessTopicListDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    AgreementId = x.AgreementId,
                    BusinessPartnerId = x.BusinessPartnerId,
                    Title = x.Title,
                    ReferenceNumber = x.ReferenceNumber,
                    TopicDate = x.TopicDate,
                    Status = x.Status,
                    CalculatedRiskAmount = x.CalculatedRiskAmount
                }).ToListAsync();
        }

        public async Task<BusinessTopicDetailDto?> GetByIdAsync(int id, int tenantId)
        {
            // Tek bir iş konusunun detayını tenant sınırı içinde getirir.
            return await _context.BusinessTopics
                .AsNoTracking()
                .Where(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted)
                .Select(x => new BusinessTopicDetailDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    AgreementId = x.AgreementId,
                    BusinessPartnerId = x.BusinessPartnerId,
                    Title = x.Title,
                    Description = x.Description,
                    ReferenceNumber = x.ReferenceNumber,
                    TopicDate = x.TopicDate,
                    Status = x.Status,
                    RawPayload = x.RawPayload,
                    CalculatedRiskAmount = x.CalculatedRiskAmount
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateBusinessTopicDto dto)
        {
            // Yeni gelen iş konusu ilk aşamada Pending statüsünde oluşturulur.
            // Risk tutarı bu aşamada kullanıcıdan alınmaz; sonradan analiz motoru hesaplar.
            var businessTopic = new BusinessTopic
            {
                TenantId = dto.TenantId,
                AgreementId = dto.AgreementId,
                BusinessPartnerId = dto.BusinessPartnerId,
                Title = dto.Title,
                Description = dto.Description,
                ReferenceNumber = dto.ReferenceNumber,
                TopicDate = dto.TopicDate,
                Status = BusinessTopicStatus.Pending,
                RawPayload = dto.RawPayload,
                CalculatedRiskAmount = null
            };

            await _context.BusinessTopics.AddAsync(businessTopic);
            await _context.SaveChangesAsync();

            // Yeni kaydın Id değeri döndürülür.
            return businessTopic.Id;
        }

        public async Task<bool> UpdateAsync(UpdateBusinessTopicDto dto)
        {
            // Güncellenecek kayıt tenant ve silinme durumuna göre bulunur.
            var businessTopic = await _context.BusinessTopics.FirstOrDefaultAsync(x => x.Id == dto.Id && x.TenantId == dto.TenantId && !x.IsDeleted);

            if (businessTopic is null)
            {
                return false;
            }

            // İş verisi güncellenir.
            // CalculatedRiskAmount burada değiştirilmez; analiz servisi tarafından yönetilir.
            businessTopic.AgreementId = dto.AgreementId;
            businessTopic.BusinessPartnerId = dto.BusinessPartnerId;
            businessTopic.Title = dto.Title;
            businessTopic.Description = dto.Description;
            businessTopic.ReferenceNumber = dto.ReferenceNumber;
            businessTopic.TopicDate = dto.TopicDate;
            businessTopic.Status = dto.Status;
            businessTopic.RawPayload = dto.RawPayload;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            // Fiziksel silme yerine soft delete uygulanır.
            var businessTopic = await _context.BusinessTopics.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted);

            if (businessTopic is null)
            {
                return false;
            }

            businessTopic.IsDeleted = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}