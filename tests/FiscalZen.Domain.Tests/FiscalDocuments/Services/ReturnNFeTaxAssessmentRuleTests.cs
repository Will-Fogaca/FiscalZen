using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.Services.TaxAssessment;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;


namespace FiscalZen.Domain.Tests.FiscalDocuments.Services;

public class ReturnNFeTaxAssessmentRuleTests
{

    [Test(Description = "Não deve adicionar os tributos da NF-e de devolução na apuração")]
    public void Should_Not_Add_Return_NFe_Taxes_To_Assessment()
    {
        // Crio uma instância de NFe de devolução

        var nfe = CreateReturnNFe();

        // Configuro os tributos dessa NFe
        nfe.SetTaxes(new TaxSummary
        {
            ICMS = new Money(100),
            IPI = new Money(50),
            PIS = new Money(20),
            COFINS = new Money(90),
            IBS = new Money(40),
            CBS = new Money(60)
        });

        // Crio uma instância das regras de apuração de uma NFe de devolução
        var rule = new ReturnNfeTaxAssessmentRule();

        // Defino o resultado do método que estou testando (Retorna um TaxSummary com os tributos somados na apuração)
        var result = rule.Assess(nfe);

        // Verifico se o resultado é o esperado, no caso desse teste, nas regras da minha apuração defini 
        // que todos os tributos não devem entrar na apuração, quando forem nota de devolução, futuramente, 
        // farei o cálculo inverso para debitar da apuração.
        Assert.Multiple(() =>
        {
            Assert.That(result.ICMS, Is.EqualTo(Money.Zero));
            Assert.That(result.IPI, Is.EqualTo(Money.Zero));
            Assert.That(result.PIS, Is.EqualTo(Money.Zero));
            Assert.That(result.COFINS, Is.EqualTo(Money.Zero));
            Assert.That(result.IBS, Is.EqualTo(Money.Zero));
            Assert.That(result.CBS, Is.EqualTo(Money.Zero));
        });

    }
    [Test(Description = "Deve identificar que a regra atende a uma NF-e de devolução")]
    public void Should_Handle_Return_Nfe()
    {
        var nfe = CreateReturnNFe();

        var rule = new ReturnNfeTaxAssessmentRule();

        var result = rule.CanHandle(nfe);

        Assert.That(result, Is.True);
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
