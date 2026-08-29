using FiscalZen.Domain.FiscalDocuments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalZen.Infrastructure.Persistence.Configurations;

public sealed class DebitNfeConfiguration : IEntityTypeConfiguration<DebitNfe>
{
    public void Configure(EntityTypeBuilder<DebitNfe> builder)
    {
        builder.Property(x => x.DebitType)
            .HasColumnName("DebitType")
            .HasConversion<int>()
            .IsRequired();
    }
}