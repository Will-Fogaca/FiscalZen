using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class NormalNFe : NFe
    {
        public override NFePurpose Purpose => NFePurpose.Normal;
        public NormalNFe(AccessKey accessKey, int number, int series, DateTime issueDate) : base(accessKey, number, series, issueDate) { }
    }
}
