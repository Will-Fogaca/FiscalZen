using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class ReturnNfeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
     
        public bool CanHandle(FiscalDocument document)
        {
            return document is ReturnNfe;
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            return TaxSummary.Zero;
        }
    }
}
