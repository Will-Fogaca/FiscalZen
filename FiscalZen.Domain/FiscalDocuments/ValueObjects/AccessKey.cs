using FiscalZen.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.ValueObjects
{
    public sealed record AccessKey
    {
        public string Value { get; }

        public AccessKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Chave de acesso não informada.");

            if (value.Length != 44)
                throw new DomainException("A chave de acesso precisa ter 44 dígitos.");

            if (!value.All(char.IsDigit))
                throw new DomainException("A chave de acesso só pode conter dígitos numéricos.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
