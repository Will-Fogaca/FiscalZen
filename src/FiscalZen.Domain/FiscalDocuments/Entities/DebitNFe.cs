using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class DebitNfe : Nfe
    {
        public override NfePurpose Purpose => NfePurpose.Debit;

        public NfeDebitType DebitType { get; }

        public DebitNfe(AccessKey accessKey, int number, int series, DateTime issueDate, NfeDebitType debitType, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime)
        {
            DebitType = debitType;
        }
    }
}
