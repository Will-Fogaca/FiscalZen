using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Enums
{
    public sealed record TaxSummary
    {
        public Money ICMS { get; init; } = Money.Zero;

        public Money IPI { get; init;  } = Money.Zero;

        public Money PIS { get; init; } = Money.Zero;

        public Money COFINS { get; init;  } = Money.Zero;

        public Money IBS { get; init;  } = Money.Zero;

        public Money CBS { get; init; } = Money.Zero;
    }
}
