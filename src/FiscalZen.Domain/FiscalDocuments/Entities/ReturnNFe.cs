using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class ReturnNfe : Nfe
    {
        public override NfePurpose Purpose => NfePurpose.Return;

        public AccessKey ReferencedAccessKey { get; }

        public ReturnNfe(AccessKey accessKey, AccessKey referencedAccessKey, int number, int series, DateTime issueDate, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime)
        {
            ReferencedAccessKey = referencedAccessKey ?? throw new DomainException("A chave de acesso do documento fiscal referenciado não foi informada.");
        }
    }
}
