using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class NFCe : FiscalDocument
    {
        public NFCe(AccessKey accessKey, int number, int series, DateTime issueDate, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime) { }
    }
}
