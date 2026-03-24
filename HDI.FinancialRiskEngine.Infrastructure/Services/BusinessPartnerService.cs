
using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;
using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Domain.Entities;
using HDI.FinancialRiskEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HDI.FinancialRiskEngine.Infrastructure.Services
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private readonly AppDbContext _context;

        public BusinessPartnerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusinessPartnerListDto>> GetAllAsync(int tenantId)
        {
            // Belirli tenant'a ait aktif/silinmemiş partnerları listeler.
            return await _context.BusinessPartners
                .AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => new BusinessPartnerListDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    AgreementId = x.AgreementId,
                    Name = x.Name,
                    Code = x.Code,
                    Email = x.Email,
                    Phone = x.Phone,
                    IsActive = x.IsActive
                }).ToListAsync();
        }

        public async Task<BusinessPartnerDetailDto?> GetByIdAsync(int id, int tenantId)
        {
            // Tek bir partnerın detayını döner.
            return await _context.BusinessPartners
                .AsNoTracking()
                .Where(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted)
                .Select(x => new BusinessPartnerDetailDto
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    AgreementId = x.AgreementId,
                    Name = x.Name,
                    Code = x.Code,
                    Email = x.Email,
                    Phone = x.Phone,
                    ApiKey = x.ApiKey, // Admin ekranında gösterilebilir
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateBusinessPartnerDto dto)
        {
            // Yeni ApiKey sistem tarafından üretilir (kullanıcıdan alınmaz).
            var apiKey = GenerateApiKey();

            var partner = new BusinessPartner
            {
                TenantId = dto.TenantId,
                AgreementId = dto.AgreementId,
                Name = dto.Name,
                Code = dto.Code,
                Email = dto.Email,
                Phone = dto.Phone,
                ApiKey = apiKey,
                IsActive = dto.IsActive
            };

            await _context.BusinessPartners.AddAsync(partner);
            await _context.SaveChangesAsync();

            return partner.Id;
        }

        public async Task<bool> UpdateAsync(UpdateBusinessPartnerDto dto)
        {
            var partner = await _context.BusinessPartners.FirstOrDefaultAsync(x => x.Id == dto.Id && x.TenantId == dto.TenantId && !x.IsDeleted);

            if (partner is null)
                return false;

            // ApiKey burada değiştirilmez (ayrı method ile yönetilir).
            partner.AgreementId = dto.AgreementId;
            partner.Name = dto.Name;
            partner.Code = dto.Code;
            partner.Email = dto.Email;
            partner.Phone = dto.Phone;
            partner.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            // Soft delete
            var partner = await _context.BusinessPartners.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted);

            if (partner is null)
                return false;

            partner.IsDeleted = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> RegenerateApiKeyAsync(int id, int tenantId)
        {
            // ApiKey yenileme ayrı bir aksiyon olarak ele alınır.
            var partner = await _context.BusinessPartners.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted);

            if (partner is null)
                throw new Exception("Partner bulunamadı.");

            var newApiKey = GenerateApiKey();

            partner.ApiKey = newApiKey;

            await _context.SaveChangesAsync();

            return newApiKey;
        }

        // Güvenli ve tahmin edilemez API key üretimi
        private static string GenerateApiKey()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
}
