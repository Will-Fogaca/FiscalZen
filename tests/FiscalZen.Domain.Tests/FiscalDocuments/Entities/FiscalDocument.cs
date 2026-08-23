using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.Entities;

public class FiscalDocumentTests
{
    [Test(Description = "Deve criar um documento fiscal com valores monetários zerados")]
    public void Should_Create_Fiscal_Document_With_Zero_Amounts()
    {
        var document = CreateDocument();

        Assert.Multiple(() =>
        {
            Assert.That(document.ProductsAmount, Is.EqualTo(Money.Zero));
            Assert.That(document.FreightAmount, Is.EqualTo(Money.Zero));
            Assert.That(document.DiscountAmount, Is.EqualTo(Money.Zero));
            Assert.That(document.TotalAmount, Is.EqualTo(Money.Zero));
        });
    }

    [Test(Description = "Não deve permitir número do documento fiscal igual ou menor que zero")]
    public void Should_Throw_When_Document_Number_Is_Invalid()
    {
        var exception = Assert.Throws<DomainException>(() => new TestFiscalDocument(CreateAccessKey(), 0, 1, DateTime.Now));
        Assert.That(exception!.Message, Is.EqualTo("O número do documento fiscal deve ser maior que zero."));
    }

    [Test(Description = "Não deve permitir série do documento fiscal menor que zero")]
    public void Should_Throw_When_Document_Series_Is_Negative()
    {
        var exception = Assert.Throws<DomainException>(() => new TestFiscalDocument(CreateAccessKey(), 1, -1, DateTime.Now));
        Assert.That(exception!.Message, Is.EqualTo("A série do documento fiscal não pode ser menor que zero."));
    }

    [Test(Description = "Não deve permitir documento fiscal sem chave de acesso")]
    public void Should_Throw_When_AccessKey_Is_Null()
    {
        var exception = Assert.Throws<DomainException>(() => new TestFiscalDocument(null!, 1, 1, DateTime.Now));
        Assert.That(exception!.Message, Is.EqualTo("A chave de acesso não foi informada."));
    }

    [Test(Description = "Deve definir o valor dos produtos")]
    public void Should_Set_Products_Amount()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));

        Assert.That(document.ProductsAmount.Value, Is.EqualTo(100));
    }

    [Test(Description = "Não deve permitir valor dos produtos negativo")]
    public void Should_Throw_When_Products_Amount_Is_Negative()
    {
        var document = CreateDocument();

        var exception = Assert.Throws<DomainException>(() => document.SetProductsAmount(new Money(-10)));

        Assert.That(exception!.Message, Is.EqualTo("O valor dos produtos não pode ser menor que zero."));
    }

    [Test(Description = "Deve definir o valor do frete")]
    public void Should_Set_Freight_Amount()
    {
        var document = CreateDocument();

        document.SetFreightAmount(new Money(25));

        Assert.That(document.FreightAmount.Value, Is.EqualTo(25));
    }

    [Test(Description = "Não deve permitir valor do frete negativo")]
    public void Should_Throw_When_Freight_Amount_Is_Negative()
    {
        var document = CreateDocument();

        var exception = Assert.Throws<DomainException>(() => document.SetFreightAmount(new Money(-10)));

        Assert.That(exception!.Message, Is.EqualTo("O valor do frete não pode ser menor que zero."));
    }

    [Test(Description = "Deve definir o valor do desconto")]
    public void Should_Set_Discount_Amount()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));
        document.SetFreightAmount(new Money(20));

        document.SetDiscountAmount(new Money(30));

        Assert.That(document.DiscountAmount.Value, Is.EqualTo(30));
    }

    [Test(Description = "Não deve permitir valor do desconto negativo")]
    public void Should_Throw_When_Discount_Amount_Is_Negative()
    {
        var document = CreateDocument();

        var exception = Assert.Throws<DomainException>(() =>
            document.SetDiscountAmount(new Money(-10)));

        Assert.That(exception!.Message, Is.EqualTo("O valor do desconto não pode ser menor que zero."));
    }

    [Test(Description = "Não deve permitir desconto maior que o valor dos produtos mais o frete")]
    public void Should_Throw_When_Discount_Is_Greater_Than_Document_Amount()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));
        document.SetFreightAmount(new Money(20));

        var exception = Assert.Throws<DomainException>(() => document.SetDiscountAmount(new Money(121)));

        Assert.That(exception!.Message, Is.EqualTo("O valor do desconto não pode ser maior que o valor do documento fiscal."));
    }

    [Test(Description = "Deve recalcular o valor total ao definir o valor dos produtos")]
    public void Should_Recalculate_Total_When_Products_Amount_Is_Set()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));

        Assert.That(document.TotalAmount.Value, Is.EqualTo(100));
    }

    [Test(Description = "Deve recalcular o valor total ao definir o frete")]
    public void Should_Recalculate_Total_When_Freight_Amount_Is_Set()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));
        document.SetFreightAmount(new Money(20));

        Assert.That(document.TotalAmount.Value, Is.EqualTo(120));
    }

    [Test(Description = "Deve recalcular o valor total ao definir o desconto")]
    public void Should_Recalculate_Total_When_Discount_Amount_Is_Set()
    {
        var document = CreateDocument();

        document.SetProductsAmount(new Money(100));
        document.SetFreightAmount(new Money(20));
        document.SetDiscountAmount(new Money(30));

        Assert.That(document.TotalAmount.Value, Is.EqualTo(90));
    }

    [Test(Description = "Deve adicionar um item ao documento fiscal")]
    public void Should_Add_Item_To_Fiscal_Document()
    {
        var document = CreateDocument();
        var item = CreateItem(1);

        document.AddItem(item);

        Assert.That(document.Items, Has.Count.EqualTo(1));
        Assert.That(document.Items.First(), Is.EqualTo(item));
    }

    [Test(Description = "Não deve permitir item nulo no documento fiscal")]
    public void Should_Throw_When_Item_Is_Null()
    {
        var document = CreateDocument();

        var exception = Assert.Throws<DomainException>(() => document.AddItem(null!));

        Assert.That(exception!.Message, Is.EqualTo("O item do documento fiscal não foi informado."));
    }

    [Test(Description = "Não deve permitir dois itens com o mesmo número")]
    public void Should_Throw_When_Item_Number_Is_Duplicated()
    {
        var document = CreateDocument();

        document.AddItem(CreateItem(1));

        var exception = Assert.Throws<DomainException>(() => document.AddItem(CreateItem(1)));

        Assert.That(exception!.Message, Is.EqualTo("Já existe um item com o número 1 no documento fiscal."));
    }

    private static TestFiscalDocument CreateDocument()
    {
        return new TestFiscalDocument(CreateAccessKey(), 123, 1, new DateTime(2026, 8, 23));
    }

    private static AccessKey CreateAccessKey()
    {
        return new AccessKey("35260812345678000190550010000012341000012345");
    }

    private static FiscalDocumentItem CreateItem(int number)
    {
        return new FiscalDocumentItem(number, "PROD001", "Produto teste", "12345678", "5102", 1, new Money(100), new Money(100));
    }

    private sealed class TestFiscalDocument : FiscalDocument
    {
        public TestFiscalDocument(AccessKey accessKey, int number, int series, DateTime issueDate) : base(accessKey, number, series, issueDate)
        {
        }
    }
}