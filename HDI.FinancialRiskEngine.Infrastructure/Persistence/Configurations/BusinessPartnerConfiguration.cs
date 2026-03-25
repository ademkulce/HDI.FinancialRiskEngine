using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Infrastructure.Persistence.Configurations
{
    public class BusinessPartnerConfiguration : IEntityTypeConfiguration<BusinessPartner>
    {
        public void Configure(EntityTypeBuilder<BusinessPartner> builder)
        {
            builder.ToTable("BusinessPartners");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .HasMaxLength(30);

            // Dış sistemlerin bu partner adına güvenli şekilde veri göndermesinde kullanılacaktır.
            builder.Property(x => x.ApiKey)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => new { x.TenantId, x.Code })
                .IsUnique();

            builder.HasIndex(x => new { x.TenantId, x.ApiKey })
                .IsUnique();

            builder.HasOne(x => x.Agreement)
                .WithMany(x => x.BusinessPartners)
                .HasForeignKey(x => x.AgreementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.BusinessPartners)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Örnek iş ortağı kaydı eklenir. Konu oluşturma ve risk analizi testlerinde hazır veri sağlar.
            builder.HasData(
                new BusinessPartner
                {
                    Id = 1,
                    TenantId = 1,
                    AgreementId = 1,
                    Name = "ABC Partner",
                    Code = "PRT001",
                    Email = "partner@test.com",
                    Phone = "05551234567",
                    ApiKey = "STATIC-SEED-API-KEY-001",
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 03, 25, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );
        }
    }
}
