using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalZen.Infrastructure.Persistence.Configurations;

public sealed class FiscalDocumentConfiguration : IEntityTypeConfiguration<FiscalDocument>
{
    public void Configure(EntityTypeBuilder<FiscalDocument> builder)
    {
        builder.ToTable("FiscalDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.Property(x => x.AccessKey)
            .HasConversion(accessKey => accessKey.Value, value => new AccessKey(value))
            .HasColumnName("AccessKey")
            .HasMaxLength(44)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.AccessKey })
            .IsUnique();

        builder.Property(x => x.Number)
            .HasColumnName("Number")
            .IsRequired();

        builder.Property(x => x.Series)
            .HasColumnName("Series")
            .IsRequired();

        builder.Property(x => x.IssueDate)
            .HasColumnName("IssueDate")
            .IsRequired();

        builder.Property(x => x.TaxRegime)
            .HasColumnName("TaxRegime")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ProductsAmount)
            .HasConversion(money => money.Value, value => new Money(value))
            .HasColumnName("ProductsAmount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.FreightAmount)
            .HasConversion(money => money.Value, value => new Money(value))
            .HasColumnName("FreightAmount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.DiscountAmount)
            .HasConversion(money => money.Value, value => new Money(value))
            .HasColumnName("DiscountAmount")
            .HasPrecision(18, 2)
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

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("FiscalDocumentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasDiscriminator<string>("DocumentType")
            .HasValue<NormalNfe>("NfeNormal")
            .HasValue<ReturnNfe>("NfeReturn")
            .HasValue<CreditNfe>("NfeCredit")
            .HasValue<DebitNfe>("NfeDebit")
            .HasValue<Nfce>("Nfce");

        builder.Property<string>("DocumentType")
            .HasColumnName("DocumentType")
            .HasMaxLength(20)
            .IsRequired();
    }
}