using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.FiscalDocuments.Entities;

public sealed class FiscalDocumentItem
{
    public int Number { get; }

    public string ProductCode { get; }

    public string Description { get; }

    public string? NCM { get; }

    public Cfop Cfop { get; }

    public decimal Quantity { get; }

    public Money UnitPrice { get; }

    public Money TotalAmount { get; }

    public TaxSummary Taxes { get; private set; }

    public FiscalDocumentItem(int number, string productCode, string description, string? ncm, Cfop cfop, decimal quantity, Money unitPrice, Money totalAmount)
    {
        if (number <= 0)
            throw new DomainException("O número do item deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(productCode))
            throw new DomainException("O código do produto não foi informado.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("A descrição do produto não foi informada.");

        if (quantity <= 0)
            throw new DomainException("A quantidade do item deve ser maior que zero.");

        UnitPrice = unitPrice ?? throw new DomainException("O valor unitário do item não foi informado.");

        TotalAmount = totalAmount ?? throw new DomainException("O valor total do item não foi informado.");

        Cfop = cfop ?? throw new DomainException("O CFOP não foi informado.");

        Number = number;
        ProductCode = productCode;
        Description = description;
        NCM = ncm;
        Quantity = quantity;
        Taxes = new TaxSummary();
    }

    public void SetTaxes(TaxSummary taxes)
    {
        Taxes = taxes ?? throw new DomainException("Os tributos do item não foram informados.");
    }
}