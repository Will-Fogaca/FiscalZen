using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;


namespace FiscalZen.Domain.Tests.FiscalDocuments.Services;

public class NormalNFeTaxAssessmentRuleTests
{
    [Test(Description = "Deve considerar os tributos informados em uma NF-e normal")]
    public void Should_Assess_Taxes_From_Normal_NFe()
    {
        var nfe = CreateNormalNFe();

        nfe.SetTaxes(new TaxSummary
        {
            ICMS = new Money(100),
            IPI = new Money(50),
            PIS = new Money(20),
            COFINS = new Money(90),
            IBS = new Money(40),
            CBS = new Money(60)
        });

        var rule = new NormalNfeTaxAssessmentRule();

        var result = rule.Assess(nfe);

        Assert.Multiple(() =>
        {
            Assert.That(result.ICMS.Value, Is.EqualTo(100));
            Assert.That(result.IPI.Value, Is.EqualTo(50));
            Assert.That(result.PIS.Value, Is.EqualTo(20));
            Assert.That(result.COFINS.Value, Is.EqualTo(90));
            Assert.That(result.IBS.Value, Is.EqualTo(40));
            Assert.That(result.CBS.Value, Is.EqualTo(60));
        });
    }

    [Test(Description = "Deve identificar que a regra atende uma NF-e normal")]
    public void Should_Handle_Normal_NFe()
    {
        var rule = new NormalNfeTaxAssessmentRule();

        var result = rule.CanHandle(CreateNormalNFe());

        Assert.That(result, Is.True);
    }

    [Test(Description = "Não deve identificar NF-e de devolução como NF-e normal")]
    public void Should_Not_Handle_Return_NFe()
    {
        var rule = new NormalNfeTaxAssessmentRule();

        var result = rule.CanHandle(CreateReturnNFe());

        Assert.That(result, Is.False);
    }

    private static NormalNfe CreateNormalNFe()
    {
        return new NormalNfe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            123,
            1,
            new DateTime(2026, 8, 25),
            TaxRegime.LucroReal
        );
    }

    private static ReturnNfe CreateReturnNFe()
    {
        return new ReturnNfe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            new AccessKey("35260812345678000190550010000056781000056789"),
            124,
            1,
            new DateTime(2026, 8, 25),
            TaxRegime.LucroReal);
    }
}