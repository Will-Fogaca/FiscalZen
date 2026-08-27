using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.ValueObjects;

public class CFOPTests
{
    [Test(Description = "Deve criar um CFOP válido")]
    public void Should_Create_Valid_CFOP()
    {
        var cfop = new Cfop("5102");

        Assert.That(cfop.Value, Is.EqualTo("5102"));
    }

    [Test(Description = "Não deve permitir CFOP vazio")]
    public void Should_Throw_When_CFOP_Is_Empty()
    {
        var exception = Assert.Throws<DomainException>(() => new Cfop(""));

        Assert.That(exception!.Message, Is.EqualTo("O CFOP não foi informado."));
    }

    [Test(Description = "Não deve permitir CFOP com quantidade de dígitos inválida")]
    public void Should_Throw_When_CFOP_Has_Invalid_Length()
    {
        var exception = Assert.Throws<DomainException>(() => new Cfop("510"));

        Assert.That(exception!.Message, Is.EqualTo("O CFOP deve possuir 4 dígitos."));
    }

    [Test(Description = "Não deve permitir CFOP com caracteres não numéricos")]
    public void Should_Throw_When_CFOP_Has_NonNumeric_Characters()
    {
        var exception = Assert.Throws<DomainException>(() => new Cfop("51A2"));

        Assert.That(exception!.Message, Is.EqualTo("O CFOP deve conter apenas números."));
    }
}