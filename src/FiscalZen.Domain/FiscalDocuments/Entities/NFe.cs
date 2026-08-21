using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public abstract class NFe : FiscalDocument
    {
       protected NFe(AccessKey accessKey, int number, int series, DateTime issueDate) : base(accessKey, number, series, issueDate){ }

       public abstract NFePurpose Purpose { get; }
    }
}
