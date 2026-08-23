using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Entities
{
    public sealed class ReturnNFe : NFe
    {
        public override NFePurpose Purpose => NFePurpose.Return;

        public AccessKey ReferencedAccessKey { get; }

        public ReturnNFe(AccessKey accessKey, AccessKey referencedAccessKey, int number, int series, DateTime issueDate) : base(accessKey, number, series, issueDate)
        {
            ReferencedAccessKey = referencedAccessKey ?? throw new DomainException("A chave de acesso do documento fiscal referenciado não foi informada.");
        }
    }
}
