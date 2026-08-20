using FiscalZen.Domain.Common.Exceptions;

namespace FiscalZen.Domain.FiscalDocuments.ValueObjects;

public sealed record Money
{
    public decimal Value { get; }

    public static Money Zero => new(0);

    public Money(decimal value)
    {
        Value = value;
    }

    public static Money operator +(Money left, Money right)
    {
        return new Money(left.Value + right.Value);
    }

    public static Money operator -(Money left, Money right)
    {
        return new Money(left.Value - right.Value);
    }

    public static bool operator >(Money left, Money right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(Money left, Money right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >=(Money left, Money right)
    {
        return left.Value >= right.Value;
    }

    public static bool operator <=(Money left, Money right)
    {
        return left.Value <= right.Value;
    }

    public override string ToString()
    {
        return Value.ToString("C2");
    }
}