using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class NormalNfeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
        public bool CanHandle(FiscalDocument document)
        {
            return document is NormalNfe;
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            return document.Taxes;
        }

       
    }
}
