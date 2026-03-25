using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Domain.Entities;
using HDI.FinancialRiskEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HDI.FinancialRiskEngine.Infrastructure.Services
{
    public class AgreementService : IAgreementService
    {
        private readonly AppDbContext _context;

        public AgreementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AgreementListDto>> GetAllAsync(int tenantId)
        {
            // Belirtilen tenant'a ait, silinmemiş anlaşmaları listeler.
            // AsNoTracking performans için kullanılır; burada veri sadece okunuyor.
            return await _context.Agreements
                .AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => new AgreementListDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    BaseRiskRate = x.BaseRiskRate,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<AgreementDetailDto?> GetByIdAsync(int id, int tenantId)
        {
            // Tek bir anlaşmanın detayını döner.
            // Tenant filtresi veri izolasyonu için özellikle eklendi.
            return await _context.Agreements
                .AsNoTracking()
                .Where(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted)
                .Select(x => new AgreementDetailDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    Code = x.Code,
                    Name = x.Name,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    BaseRiskRate = x.BaseRiskRate,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateAgreementDto dto)
        {
            // DTO verisi entity'ye dönüştürülür ve yeni kayıt oluşturulur.
            var agreement = new Agreement
            {
                TenantId = dto.TenantId,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                BaseRiskRate = dto.BaseRiskRate,
                IsActive = dto.IsActive
            };

            await _context.Agreements.AddAsync(agreement);
            await _context.SaveChangesAsync();

            // Oluşan kaydın Id bilgisi geri döndürülür.
            return agreement.Id;
        }

        public async Task<bool> UpdateAsync(UpdateAgreementDto dto)
        {
            // Güncellenecek kayıt tenant ve silinme durumuna göre bulunur.
            var agreement = await _context.Agreements.FirstOrDefaultAsync(x => x.Id == dto.Id && x.TenantId == dto.TenantId && !x.IsDeleted);

            if (agreement is null)
            {
                return false;
            }

            // Entity alanları DTO içeriği ile güncellenir.
            agreement.Code = dto.Code;
            agreement.Name = dto.Name;
            agreement.Description = dto.Description;
            agreement.StartDate = dto.StartDate;
            agreement.EndDate = dto.EndDate;
            agreement.BaseRiskRate = dto.BaseRiskRate;
            agreement.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            // Fiziksel silme yerine soft delete uygulanır.
            // Böylece kayıt tamamen kaybolmaz, audit ve geri dönüş imkanı korunur.
            var agreement = await _context.Agreements.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted);

            if (agreement is null)
            {
                return false;
            }

            agreement.IsDeleted = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}