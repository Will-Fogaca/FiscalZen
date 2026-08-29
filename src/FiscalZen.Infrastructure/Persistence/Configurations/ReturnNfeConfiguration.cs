using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalZen.Infrastructure.Persistence.Configurations;

public sealed class ReturnNfeConfiguration : IEntityTypeConfiguration<ReturnNfe>
{
    public void Configure(EntityTypeBuilder<ReturnNfe> builder)
    {
        builder.Property(x => x.ReferencedAccessKey)
            .HasConversion(
                accessKey => accessKey.Value,
                value => new AccessKey(value))
            .HasColumnName("ReferencedAccessKey")
            .HasMaxLength(44)
            .IsRequired();
    }
}