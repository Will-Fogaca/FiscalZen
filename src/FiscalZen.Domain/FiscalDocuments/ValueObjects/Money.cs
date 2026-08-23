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

    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Value * multiplier);
    }

    public static Money operator *(decimal multiplier, Money money)
    {
        return money * multiplier;
    }

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Não é possível dividir um valor monetário por zero.");

        return new Money(money.Value / divisor);
    }

    public static decimal operator /(Money left, Money right)
    {
        if (right.Value == 0)
            throw new DivideByZeroException("Não é possível dividir por um valor monetário igual a zero.");

        return left.Value / right.Value;
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