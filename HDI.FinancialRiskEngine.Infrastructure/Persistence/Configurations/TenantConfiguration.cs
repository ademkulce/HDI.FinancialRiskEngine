using HDI.FinancialRiskEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
                .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        // Uygulama ilk ayağa kalktığında örnek tenant verisi eklenir.
        builder.HasData(
            new Tenant
            {
                Id = 1,
                Name = "HDI Default Tenant",
                Code = "HDI",
                IsActive = true,
                CreatedDate = new DateTime(2026, 03, 25, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}