using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class DebitNfeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
        public bool CanHandle(FiscalDocument document)
        {
            return document is DebitNfe;
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            var nfe = (DebitNfe)document;

            throw new NotImplementedException($"A regra de apuração para NF-e de débito do tipo {nfe.DebitType} ainda não foi implementada.");
        }
    }
}
