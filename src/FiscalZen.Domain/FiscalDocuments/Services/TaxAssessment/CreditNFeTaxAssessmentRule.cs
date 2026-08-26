using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;


namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment
{
    public sealed class CreditNFeTaxAssessmentRule : IFiscalDocumentTaxAssessmentRule
    {
       
        public bool CanHandle(FiscalDocument document)
        {
            return (document is CreditNFe);
        }

        public TaxSummary Assess(FiscalDocument document)
        {
            var nfe = (CreditNFe)document;

            throw new NotImplementedException($"A regra de apuração para NF-e de crédito do tipo {nfe.CreditType} ainda não foi implementada.");
        }
         
    }
}
