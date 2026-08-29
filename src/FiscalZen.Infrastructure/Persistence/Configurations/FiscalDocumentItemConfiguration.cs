using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalZen.Infrastructure.Persistence.Configurations;

public sealed class FiscalDocumentItemConfiguration : IEntityTypeConfiguration<FiscalDocumentItem>
{
    public void Configure(EntityTypeBuilder<FiscalDocumentItem> builder)
    {
        builder.ToTable("FiscalDocumentItems");

        builder.HasKey("FiscalDocumentId", nameof(FiscalDocumentItem.Number));

        builder.Property<Guid>("FiscalDocumentId")
            .HasColumnName("FiscalDocumentId")
            .IsRequired();

        builder.Property(x => x.Number)
            .HasColumnName("Number")
            .IsRequired();

        builder.Property(x => x.ProductCode)
            .HasColumnName("ProductCode")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Ncm)
            .HasConversion(ncm => ncm.Value, value => new Ncm(value))
            .HasColumnName("Ncm")
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Cfop)
            .HasConversion(cfop => cfop.Value, value => new Cfop(value))
            .HasColumnName("Cfop")
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasConversion(money => money.Value, value => new Money(value))
            .HasColumnName("UnitPrice")
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasConversion(money => money.Value, value => new Money(value))
            .HasColumnName("TotalAmount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.OwnsOne(x => x.Taxes, taxes =>
        {
            taxes.Property(x => x.ICMS)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("ICMS")
                .HasPrecision(18, 2)
                .IsRequired();

            taxes.Property(x => x.IPI)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("IPI")
                .HasPrecision(18, 2)
                .IsRequired();

            taxes.Property(x => x.PIS)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("PIS")
                .HasPrecision(18, 2)
                .IsRequired();

            taxes.Property(x => x.COFINS)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("COFINS")
                .HasPrecision(18, 2)
                .IsRequired();

            taxes.Property(x => x.IBS)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("IBS")
                .HasPrecision(18, 2)
                .IsRequired();

            taxes.Property(x => x.CBS)
                .HasConversion(money => money.Value, value => new Money(value))
                .HasColumnName("CBS")
                .HasPrecision(18, 2)
                .IsRequired();
        });
    }
}