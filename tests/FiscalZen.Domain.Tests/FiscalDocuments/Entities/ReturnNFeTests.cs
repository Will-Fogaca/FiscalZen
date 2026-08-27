using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.Entities;

public class ReturnNFeTests
{
    [Test(Description = "Deve criar uma NF-e de devolução com chave de acesso e chave referenciada válidas")]
    public void Should_Create_Return_NFe_With_Valid_Access_Keys()
    {
        var accessKey = CreateAccessKey();

        var referencedAccessKey = CreateReferencedAccessKey();

        var nfe = new ReturnNfe(
            accessKey,
            referencedAccessKey,
            123,
            1,
            new DateTime(2026, 8, 23), 
            TaxRegime.LucroReal);

        Assert.Multiple(() =>
        {
            Assert.That(nfe.AccessKey, Is.EqualTo(accessKey));
            Assert.That(nfe.ReferencedAccessKey, Is.EqualTo(referencedAccessKey));
        });
    }

    [Test(Description = "Não deve permitir NF-e de devolução sem chave de acesso")]
    public void Should_Throw_When_Return_NFe_Access_Key_Is_Null()
    {
        var referencedAccessKey = CreateReferencedAccessKey();

        var exception = Assert.Throws<DomainException>(() => new ReturnNfe(null!, referencedAccessKey, 123, 1, DateTime.Now, TaxRegime.LucroReal));

        Assert.That(exception!.Message, Is.EqualTo("A chave de acesso não foi informada."));
    }

    [Test(Description = "Não deve permitir NF-e de devolução sem chave de acesso referenciada")]
    public void Should_Throw_When_Referenced_Access_Key_Is_Null()
    {
        var accessKey = CreateAccessKey();

        var exception = Assert.Throws<DomainException>(() => new ReturnNfe(accessKey, null!, 123, 1, DateTime.Now, TaxRegime.LucroReal));

        Assert.That(exception!.Message, Is.EqualTo("A chave de acesso do documento fiscal referenciado não foi informada."));
    }

    private static AccessKey CreateAccessKey()
    {
        return new AccessKey("35260812345678000190550010000012341000012345");
    }

    private static AccessKey CreateReferencedAccessKey()
    {
        return new AccessKey("35260812345678000190550010000056781000056789");
    }
}