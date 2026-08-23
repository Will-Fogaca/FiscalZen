using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.Entities;

public class ReturnNFeTests
{
    [Test(Description = "Deve criar uma NF-e de devolução com chave de acesso e chave referenciada válidas")]
    public void Should_Create_Return_NFe_With_Valid_Access_Keys()
    {
        var accessKey = new AccessKey("35260812345678000190550010000012341000012345");

        var referencedAccessKey = new AccessKey("35260812345678000190550010000056781000056789");

        var nfe = new ReturnNFe(accessKey, referencedAccessKey, 123, 1, new DateTime(2026, 8, 23));

        Assert.Multiple(() =>
        {
            Assert.That(nfe.AccessKey, Is.EqualTo(accessKey));
            Assert.That(nfe.ReferencedAccessKey, Is.EqualTo(referencedAccessKey));
        });
    }

    [Test(Description = "Não deve permitir NF-e de devolução sem chave de acesso")]
    public void Should_Throw_When_Return_NFe_Access_Key_Is_Null()
    {
        var referencedAccessKey = new AccessKey("35260812345678000190550010000056781000056789");

        var exception = Assert.Throws<DomainException>(() => new ReturnNFe(null!, referencedAccessKey, 123, 1, new DateTime(2026, 8, 23)));

        Assert.That(exception!.Message,Is.EqualTo("A chave de acesso não foi informada."));
    }

    [Test(Description = "Não deve permitir NF-e de devolução sem chave de acesso referenciada")]
    public void Should_Throw_When_Referenced_Access_Key_Is_Null()
    {
        var accessKey = new AccessKey("35260812345678000190550010000012341000012345");

        var exception = Assert.Throws<DomainException>(() => new ReturnNFe(accessKey, null!, 123, 1, new DateTime(2026, 8, 23)));

        Assert.That(exception!.Message, Is.EqualTo("A chave de acesso do documento fiscal referenciado não foi informada."));
    }
}