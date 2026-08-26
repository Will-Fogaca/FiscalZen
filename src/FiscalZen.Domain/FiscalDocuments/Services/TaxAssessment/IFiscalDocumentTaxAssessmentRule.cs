using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public interface IFiscalDocumentTaxAssessmentRule
    {
        bool CanHandle(FiscalDocument document);

        TaxSummary Assess(FiscalDocument document);
    }
}
