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
            // İlgili tenant altında agreement gerçekten var mı kontrol edilir.
            var agreementExists = await _context.Agreements.AnyAsync(x => x.Id == dto.AgreementId && x.TenantId == dto.TenantId && !x.IsActive && x.IsDeleted);

            if (!agreementExists) {

                throw new InvalidOperationException("Belirtilen AgreementId geçerli değil. Anlaşma aktif ve silinmemiş olmalıdır.");
            }

            var partnerExists = await _context.BusinessPartners.FirstOrDefaultAsync(x => x.Id == dto.BusinessPartnerId && x.TenantId == dto.TenantId && !x.IsDeleted && x.IsActive);

            if (partnerExists is null)
            {
                throw new InvalidOperationException("Belirtilen BusinessPartnerId geçerli değil. İş ortağı aktif ve silinmemiş olmalıdır.");
            }

            // Partner ile agreement eşleşiyor mu kontrol edilir.Böylece farklı agreement'a bağlı partner ile topic açılması engellenir.
            if (partnerExists.AgreementId != dto.AgreementId)
            {
                throw new InvalidOperationException("Seçilen iş ortağı, belirtilen anlaşma ile eşleşmiyor. ");
            }

            // Aynı tenant altında aynı reference number ile tekrar kayıt açılmasını engellemek istersek  burada kontrol edebiliriz. Şimdilik reference number doluysa kontrol yapıyoruz.
            if (!string.IsNullOrWhiteSpace(dto.ReferenceNumber))
            {
                var referenceExists = await _context.BusinessTopics.AnyAsync(x => x.TenantId == dto.TenantId && x.ReferenceNumber == dto.ReferenceNumber && !x.IsDeleted);

                if (referenceExists)
                {
                    throw new Exception("Aynı referans numarası ile daha önce kayıt oluşturulmuş.");
                }
            }

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