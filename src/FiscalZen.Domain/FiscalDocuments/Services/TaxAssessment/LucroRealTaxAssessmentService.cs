using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment;

public sealed class LucroRealTaxAssessmentService
{
    private readonly IEnumerable<IFiscalDocumentTaxAssessmentRule> _rules;

    public LucroRealTaxAssessmentService(IEnumerable<IFiscalDocumentTaxAssessmentRule> rules)
    {
        _rules = rules ?? throw new DomainException("As regras de apuração não foram informadas.");
    }

    public TaxSummary Assess(IEnumerable<FiscalDocument> documents)
    {
        if (documents is null)
            throw new DomainException("Os documentos fiscais não foram informados.");

        var assessment = TaxSummary.Zero;

        foreach (var document in documents)
        {
            var rule = _rules.FirstOrDefault(x => x.CanHandle(document));

            if (rule is null)
                throw new DomainException($"Não existe uma regra de apuração para o documento fiscal {document.AccessKey}.");

            var documentAssessment = rule.Assess(document);

            assessment += documentAssessment;
        }

        return assessment;
    }   
}