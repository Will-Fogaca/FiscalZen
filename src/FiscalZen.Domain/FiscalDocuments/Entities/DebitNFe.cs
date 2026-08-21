using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class DebitNFe : NFe
    {
        public override NFePurpose Purpose => NFePurpose.Debit;

        public NFeDebitType DebitType { get; }

        public DebitNFe(AccessKey accessKey, int number, int series, DateTime issueDate, NFeDebitType debitType) : base(accessKey, number, series, issueDate)
        {
            DebitType = debitType;
        }
    }
}
