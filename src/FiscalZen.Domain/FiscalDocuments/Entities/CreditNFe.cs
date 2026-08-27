using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class CreditNFe : NFe
    {
        public override NFePurpose Purpose => NFePurpose.Credit;

        public NFeCreditType CreditType { get; }

        public CreditNFe(AccessKey accessKey, int number, int series, DateTime issueDate, NFeCreditType creditType, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime)
        {
            CreditType = creditType;
        }
    }
}
