using FiscalZen.Domain.Common.Abstractions;
using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.FiscalDocuments.Entities;

public abstract class FiscalDocument : IAggregateRoot
{
    private readonly List<FiscalDocumentItem> _items = [];

    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }

    public AccessKey AccessKey { get; }
    public int Number { get; }
    public int Series { get; }
    public DateTime IssueDate { get; }

    public Money ProductsAmount { get; private set; }
    public Money FreightAmount { get; private set; }
    public Money DiscountAmount { get; private set; }
    public Money TotalAmount { get; private set; }

    public TaxRegime TaxRegime { get; }
    public TaxSummary Taxes { get; private set; }

    public IReadOnlyCollection<FiscalDocumentItem> Items => _items;

    protected FiscalDocument(
        AccessKey accessKey,
        int number,
        int series,
        DateTime issueDate,
        TaxRegime taxRegime)
    {
        if (number <= 0)
            throw new DomainException("O número do documento fiscal deve ser maior que zero.");

        if (series < 0)
            throw new DomainException("A série do documento fiscal não pode ser menor que zero.");

        Id = Guid.NewGuid();

        AccessKey = accessKey ?? throw new DomainException("A chave de acesso não foi informada.");
        Number = number;
        Series = series;
        IssueDate = issueDate;
        TaxRegime = taxRegime;

        ProductsAmount = Money.Zero;
        FreightAmount = Money.Zero;
        DiscountAmount = Money.Zero;
        TotalAmount = Money.Zero;
        Taxes = new TaxSummary();
    }

    public void AssignAccount(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new DomainException("A conta não foi informada.");

        AccountId = accountId;
    }

    public void AddItem(FiscalDocumentItem item)
    {
        if (item is null)
            throw new DomainException("O item do documento fiscal não foi informado.");

        if (_items.Any(x => x.Number == item.Number))
            throw new DomainException($"Já existe um item com o número {item.Number} no documento fiscal.");

        _items.Add(item);
    }

    public void SetProductsAmount(Money amount)
    {
        EnsureNonNegative(amount, "O valor dos produtos");

        ProductsAmount = amount;
    }

    public void SetFreightAmount(Money amount)
    {
        EnsureNonNegative(amount, "O valor do frete");

        FreightAmount = amount;
    }

    public void SetDiscountAmount(Money amount)
    {
        EnsureNonNegative(amount, "O valor do desconto");

        if (amount > ProductsAmount + FreightAmount)
            throw new DomainException("O valor do desconto não pode ser maior que o valor do documento fiscal.");

        DiscountAmount = amount;
    }

    public void SetTotalAmount(Money amount)
    {
        EnsureNonNegative(amount, "O valor total");

        TotalAmount = amount;
    }

    public void SetTaxes(TaxSummary taxes)
    {
        Taxes = taxes ?? throw new DomainException("Os tributos do documento fiscal não foram informados.");
    }

    private static void EnsureNonNegative(Money amount, string field)
    {
        if (amount is null)
            throw new DomainException($"{field} não foi informado.");

        if (amount.Value < 0)
            throw new DomainException($"{field} não pode ser menor que zero.");
    }
}