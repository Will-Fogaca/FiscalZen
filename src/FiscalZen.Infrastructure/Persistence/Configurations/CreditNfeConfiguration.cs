using FiscalZen.Domain.FiscalDocuments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalZen.Infrastructure.Persistence.Configurations;

public sealed class CreditNfeConfiguration : IEntityTypeConfiguration<CreditNfe>
{
    public void Configure(EntityTypeBuilder<CreditNfe> builder)
    {
        builder.Property(x => x.CreditType)
            .HasColumnName("CreditType")
            .HasConversion<int>()
            .IsRequired();
    }
}