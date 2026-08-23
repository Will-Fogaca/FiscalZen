using FiscalZen.Domain.Common.Exceptions;
using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.ValueObjects
{
    public sealed record CFOP
    {
        public string Value { get; }

        public CFOP(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("O CFOP não foi informado.");
            }

            if (value.Length != 4) 
            {
                throw new DomainException("O CFOP deve possuir 4 dígitos.");
            }

            if (!value.All(char.IsDigit))
            {
                throw new DomainException("O CFOP deve conter apenas números.");
            }

            Value = value;
        }

        public override 
            string ToString()
        {
            return Value;
        }
    }
}
