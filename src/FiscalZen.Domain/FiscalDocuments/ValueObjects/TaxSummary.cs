namespace FiscalZen.Domain.FiscalDocuments.ValueObjects;

public sealed record TaxSummary
{
    public Money ICMS { get; init; } = Money.Zero;

    public Money IPI { get; init; } = Money.Zero;

    public Money PIS { get; init; } = Money.Zero;

    public Money COFINS { get; init; } = Money.Zero;

    public Money IBS { get; init; } = Money.Zero;

    public Money CBS { get; init; } = Money.Zero;

    public static TaxSummary Zero => new();

    public static TaxSummary operator +(TaxSummary left, TaxSummary right)
    {
        return new TaxSummary
        {
            ICMS = left.ICMS + right.ICMS,
            IPI = left.IPI + right.IPI,
            PIS = left.PIS + right.PIS,
            COFINS = left.COFINS + right.COFINS,
            IBS = left.IBS + right.IBS,
            CBS = left.CBS + right.CBS
        };
    }
}