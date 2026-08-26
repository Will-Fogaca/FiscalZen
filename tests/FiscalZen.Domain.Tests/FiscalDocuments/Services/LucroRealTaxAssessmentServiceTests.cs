using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;


namespace FiscalZen.Domain.Tests.FiscalDocuments.Services;

public class LucroRealTaxAssessmentServiceTests
{
    [Test(Description = "Deve somar os tributos de múltiplas NF-e normais")]
    public void Should_Sum_Taxes_From_Multiple_Normal_NFe()
    {
        var first = CreateNormalNFe(123);
        first.SetTaxes(new TaxSummary
        {
            ICMS = new Money(100),
            IPI = new Money(50),
            PIS = new Money(20),
            COFINS = new Money(90),
            IBS = new Money(40),
            CBS = new Money(60)
        });

        var second = CreateNormalNFe(124);
        second.SetTaxes(new TaxSummary
        {
            ICMS = new Money(200),
            IPI = new Money(30),
            PIS = new Money(10),
            COFINS = new Money(40),
            IBS = new Money(20),
            CBS = new Money(30)
        });

        var rules = new IFiscalDocumentTaxAssessmentRule[]
        {
            new NormalNFeTaxAssessmentRule(),
            new ReturnNFeTaxAssessmentRule()
        };

        var service = new LucroRealTaxAssessmentService(rules);

        var result = service.Assess(new FiscalDocument[]
        {
            first,
            second
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.ICMS.Value, Is.EqualTo(300));
            Assert.That(result.IPI.Value, Is.EqualTo(80));
            Assert.That(result.PIS.Value, Is.EqualTo(30));
            Assert.That(result.COFINS.Value, Is.EqualTo(130));
            Assert.That(result.IBS.Value, Is.EqualTo(60));
            Assert.That(result.CBS.Value, Is.EqualTo(90));
        });
    }

    [Test(Description = "Não deve somar os tributos de uma NF-e de devolução à apuração")]
    public void Should_Ignore_Return_NFe_Taxes_In_Assessment()
    {
        var normal = CreateNormalNFe(123);

        normal.SetTaxes(new TaxSummary
        {
            IPI = new Money(100),
            PIS = new Money(20)
        });

        var returnNFe = CreateReturnNFe();

        returnNFe.SetTaxes(new TaxSummary
        {
            IPI = new Money(50),
            PIS = new Money(10)
        });

        var rules = new IFiscalDocumentTaxAssessmentRule[]
        {
            new NormalNFeTaxAssessmentRule(),
            new ReturnNFeTaxAssessmentRule()
        };

        var service = new LucroRealTaxAssessmentService(rules);

        var result = service.Assess(new FiscalDocument[]
        {
            normal,
            returnNFe
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.IPI.Value, Is.EqualTo(100));
            Assert.That(result.PIS.Value, Is.EqualTo(20));
        });
    }

    [Test(Description = "Não deve realizar apuração sem documentos fiscais")]
    public void Should_Throw_When_Documents_Are_Null()
    {
        var rules = new IFiscalDocumentTaxAssessmentRule[]
        {
            new NormalNFeTaxAssessmentRule()
        };

        var service = new LucroRealTaxAssessmentService(rules);

        var exception = Assert.Throws<DomainException>(() => service.Assess(null!));

        Assert.That(exception!.Message, Is.EqualTo("Os documentos fiscais não foram informados."));
    }

    [Test(Description = "Não deve realizar apuração quando não existir regra para o documento fiscal")]
    public void Should_Throw_When_No_Assessment_Rule_Exists()
    {
        var rules = Array.Empty<IFiscalDocumentTaxAssessmentRule>();

        var service = new LucroRealTaxAssessmentService(rules);

        var nfe = CreateNormalNFe(123);

        var exception = Assert.Throws<DomainException>(() => service.Assess(new FiscalDocument[] { nfe }));

        Assert.That(exception!.Message, Does.Contain("Não existe uma regra de apuração"));
    }

    private static NormalNFe CreateNormalNFe(int number)
    {
        return new NormalNFe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            number,
            1,
            new DateTime(2026, 8, 25));
    }

    private static ReturnNFe CreateReturnNFe()
    {
        return new ReturnNFe(
            new AccessKey("35260812345678000190550010000012341000012345"),
            new AccessKey("35260812345678000190550010000056781000056789"),
            125,
            1,
            new DateTime(2026, 8, 25));
    }
}