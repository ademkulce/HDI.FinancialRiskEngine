using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Infrastructure.Persistence.Configurations
{
    public class AgreementConfiguration : IEntityTypeConfiguration<Agreement>
    {
        public void Configure(EntityTypeBuilder<Agreement> builder)
        {
            builder.ToTable("Agreements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.BaseRiskRate)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Unique: Aynı tenant içinde aynı code olmasın diye kontrol
            builder.HasIndex(x => new { x.TenantId, x.Code })
                .IsUnique();

            // Tenant ilişkisi
            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.Agreements)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Örnek agreement kaydı eklenir bu sayede sistem ilk çalıştığında temel ilişki verileri hazır olur.
            builder.HasData(
                new Agreement
                {
                    Id = 1,
                    TenantId = 1,
                    Code = "AGR001",
                    Name = "Genel Finansal Risk Sözleşmesi",
                    Description = "Sistem başlangıcı için oluşturulmuş örnek agreement kaydı.",
                    StartDate = new DateTime(2026, 01, 01),
                    EndDate = new DateTime(2026, 12, 31),
                    BaseRiskRate = 1.50m,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 03, 25, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            );

        }
    }
}
