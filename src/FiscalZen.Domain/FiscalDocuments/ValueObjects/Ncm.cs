using FiscalZen.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.ValueObjects
{
    public sealed record Ncm
    {
        public string Value { get; }

        public Ncm(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("O NCM não foi informado.");

            if (value.Length != 8)
                throw new DomainException("O NCM deve possuir 8 dígitos.");

            if (!value.All(char.IsDigit))
                throw new DomainException("O NCM deve conter apenas números.");

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
