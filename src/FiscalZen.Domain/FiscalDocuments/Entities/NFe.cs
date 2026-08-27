using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public abstract class Nfe : FiscalDocument
    {
       protected Nfe(AccessKey accessKey, int number, int series, DateTime issueDate, TaxRegime taxRegime) : base(accessKey, number, series, issueDate, taxRegime){ }

       public abstract NfePurpose Purpose { get; }
    }
}
