using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Infrastructure.Persistence.Configurations
{
    public class BusinessTopicConfiguration : IEntityTypeConfiguration<BusinessTopic>
    {
        public void Configure(EntityTypeBuilder<BusinessTopic> builder)
        {
            builder.ToTable("BusinessTopics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.Description)
                .IsRequired();

            // Dış sistemden gelen kayıtların eşleştirilmesi için kullanılabilir.
            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasConversion<int>();

            builder.Property(x => x.CalculatedRiskAmount)
                .HasColumnType("decimal(18,2)");

            // Sorgu performansını artırmak için sık filtrelenecek alanlara index eklendi.
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => x.AgreementId);
            builder.HasIndex(x => x.BusinessPartnerId);
            builder.HasIndex(x => x.TopicDate);

            builder.HasOne(x => x.Agreement)
                .WithMany(x => x.BusinessTopics)
                .HasForeignKey(x => x.AgreementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BusinessPartner)
                .WithMany(x => x.BusinessTopics)
                .HasForeignKey(x => x.BusinessPartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
