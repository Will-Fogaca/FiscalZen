using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class CreditNfe : Nfe
    {
        public override NfePurpose Purpose => NfePurpose.Credit;

        public NfeCreditType CreditType { get; }

        public CreditNfe(AccessKey accessKey, int number, int series, DateTime issueDate, NfeCreditType creditType, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime)
        {
            CreditType = creditType;
        }
    }
}
