using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class DebitNFeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
        public bool CanHandle(FiscalDocument document)
        {
            return document is DebitNFe;
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            var nfe = (DebitNFe)document;

            throw new NotImplementedException($"A regra de apuração para NF-e de débito do tipo {nfe.DebitType} ainda não foi implementada.");
        }
    }
}
