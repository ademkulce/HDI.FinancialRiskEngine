using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Infrastructure.Persistence.Configurations
{
    public class AgreementKeywordConfiguration : IEntityTypeConfiguration<AgreementKeyword>
    {
        public void Configure(EntityTypeBuilder<AgreementKeyword> builder)
        {
            builder.ToTable("AgreementKeywords");

            builder.HasKey(x => x.Id);

            // Anahtar kelime alanı zorunludur ve maksimum uzunluğu 150 karakterdir.
            builder.Property(x => x.Keyword)
                .IsRequired()
                .HasMaxLength(150);

            // Risk ağırlığı finansal/hesaplamalı bir alan olduğu için decimal olarak tanımlanır.
            builder.Property(x => x.Weight)
                .HasColumnType("decimal(18,2)");

            // Aynı anlaşma altında aynı anahtar kelimenin birden fazla kez eklenmesini engeller.
            // Örnek: AgreementId=5 için "ceza" kelimesi yalnızca 1 kez bulunabilir.
            builder.HasIndex(x => new { x.AgreementId, x.Keyword })
                .IsUnique();

            // Her keyword kaydı belirli bir tenant'a bağlıdır.
            builder.HasOne(x => x.Agreement)
                .WithMany(x => x.AgreementKeywords)
                .HasForeignKey(x => x.AgreementId)
                .OnDelete(DeleteBehavior.Restrict);

            // Her keyword kaydı belirli bir tenant'a bağlıdır.
            builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
