using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDI.FinancialRiskEngine.Infrastructure.Persistence.Configurations
{
    public class RiskAnalysisConfiguration : IEntityTypeConfiguration<RiskAnalysis>
    {
        public void Configure(EntityTypeBuilder<RiskAnalysis> builder)
        {
            builder.ToTable("RiskAnalyses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MatchedKeywords)
                .HasMaxLength(1000);

            builder.Property(x => x.RiskAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.RiskLevel)
                .HasConversion<int>();

            builder.Property(x => x.AnalysisNotes)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.BusinessTopicId)
                .IsUnique();

            builder.HasOne(x => x.BusinessTopic)
                .WithOne(x => x.RiskAnalysis)
                .HasForeignKey<RiskAnalysis>(x => x.BusinessTopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
