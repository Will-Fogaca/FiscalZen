using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class NormalNFeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
        public bool CanHandle(FiscalDocument document)
        {
            return document is NormalNFe;
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            return document.Taxes;
        }

       
    }
}
