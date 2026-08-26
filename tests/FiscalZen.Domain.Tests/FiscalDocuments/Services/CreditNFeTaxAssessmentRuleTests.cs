using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.Services;

public class CreditNFeTaxAssessmentRuleTests
{
  
    [Test(Description = "Deve identificar que a regra atende a uma NF-e de crédito")]
    public void Should_Handle_Credit_NFe()
    {
        var nfe = CreateCreditNFe();

        var rule = new CreditNFeTaxAssessmentRule();
        
        var result = rule.CanHandle(nfe);

        Assert.That(result, Is.True);
    }

    [Test(Description = "Deve informar quando as regras de apuração da NFe de crédito ainda não estiverem implementadas. ")]
    public void Should_Throw_When_Credit_NFe_Assessment_Is_Not_Implemented()
    {
        var nfe = CreateCreditNFe();

        var rule = new CreditNFeTaxAssessmentRule();

        Assert.Throws<NotImplementedException>(() => rule.Assess(nfe));
    }

    [Test(Description = "Não deve identificar uma NFe normal como uma NFe de crédito na apuração.")]
    public void Should_Not_Handle_Normal_NFe()
    {
        var nfe = CreateNormalNFe();

        var rule = new CreditNFeTaxAssessmentRule();

        var result = rule.CanHandle(nfe);

        Assert.That(result, Is.False);
    }


    private static CreditNFe CreateCreditNFe()
    {
        return new CreditNFe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            124,
            1,
            new DateTime(2026, 8, 25),
            NFeCreditType.ValueReduction
        );
    }

    private static NormalNFe CreateNormalNFe()
    {
        return new NormalNFe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            124,
            1,
            new DateTime(2026, 8, 25)
        );
    }
}
