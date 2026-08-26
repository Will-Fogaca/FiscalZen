using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.ValueObjects;

public class AccessKeyTests
{
    [Test(Description = "Deve criar uma chave de acesso válida")]
    public void Should_Create_Valid_AccessKey()
    {
        var value = "35260812345678000190550010000012341000012345";

        var accessKey = new AccessKey(value);

        Assert.That(accessKey.Value, Is.EqualTo(value), "A chave de acesso criada não corresponde ao valor informado.");
    }

    [Test(Description = "Não deve permitir uma chave de acesso vazia")]
    public void Should_Throw_When_AccessKey_Is_Empty()
    {
        Assert.Throws<DomainException>(() => new AccessKey(""));
    }

    [Test(Description = "Não deve permitir uma chave de acesso com menos de 44 dígitos")]
    public void Should_Throw_When_AccessKey_Has_Invalid_Length()
    {
        Assert.Throws<DomainException>(() => new AccessKey("123456789"));
    }

    [Test(Description = "Não deve permitir caracteres não numéricos na chave de acesso")]
    public void Should_Throw_When_AccessKey_Has_NonNumeric_Characters()
    {
        var value = "3526081234567800019055001000001234100001234A";

        Assert.Throws<DomainException>(() => new AccessKey(value));
    }
}