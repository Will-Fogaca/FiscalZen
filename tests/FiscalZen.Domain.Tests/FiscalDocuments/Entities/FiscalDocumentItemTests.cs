using FiscalZen.Domain.Common.Exceptions;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests.FiscalDocuments.Entities;

public class FiscalDocumentItemTests
{
    [Test(Description = "Deve criar um item de documento fiscal válido")]
    public void Should_Create_Valid_Fiscal_Document_Item()
    {
        var item = CreateItem();

        Assert.Multiple(() =>
        {
            Assert.That(item.Number, Is.EqualTo(1));
            Assert.That(item.ProductCode, Is.EqualTo("PROD001"));
            Assert.That(item.Description, Is.EqualTo("Produto teste"));
            Assert.That(item.NCM, Is.EqualTo("12345678"));
            Assert.That(item.CFOP.Value, Is.EqualTo("5102"));
            Assert.That(item.Quantity, Is.EqualTo(2));
            Assert.That(item.UnitPrice.Value, Is.EqualTo(50));
            Assert.That(item.TotalAmount.Value, Is.EqualTo(100));
        });
    }

    [Test(Description = "Não deve permitir número do item igual ou menor que zero")]
    public void Should_Throw_When_Item_Number_Is_Invalid()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                0,
                "PROD001",
                "Produto teste",
                "12345678",
                new CFOP("5102"),
                2,
                new Money(50),
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("O número do item deve ser maior que zero."));
    }

    [Test(Description = "Não deve permitir código do produto vazio")]
    public void Should_Throw_When_Product_Code_Is_Empty()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "",
                "Produto teste",
                "12345678",
                new CFOP("5102"),
                2,
                new Money(50),
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("O código do produto não foi informado."));
    }

    [Test(Description = "Não deve permitir descrição do produto vazia")]
    public void Should_Throw_When_Description_Is_Empty()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "PROD001",
                "",
                "12345678",
                new CFOP("5102"),
                2,
                new Money(50),
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("A descrição do produto não foi informada."));
    }

    [Test(Description = "Não deve permitir CFOP nulo")]
    public void Should_Throw_When_CFOP_Is_Null()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "PROD001",
                "Produto teste",
                "12345678",
                null!,
                2,
                new Money(50),
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("O CFOP não foi informado."));
    }

    [Test(Description = "Não deve permitir quantidade igual ou menor que zero")]
    public void Should_Throw_When_Quantity_Is_Invalid()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "PROD001",
                "Produto teste",
                "12345678",
                new CFOP("5102"),
                0,
                new Money(50),
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("A quantidade do item deve ser maior que zero."));
    }

    [Test(Description = "Não deve permitir valor unitário nulo")]
    public void Should_Throw_When_Unit_Price_Is_Null()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "PROD001",
                "Produto teste",
                "12345678",
                new CFOP("5102"),
                2,
                null!,
                new Money(100)));

        Assert.That(exception!.Message, Is.EqualTo("O valor unitário do item não foi informado."));
    }

    [Test(Description = "Não deve permitir valor total nulo")]
    public void Should_Throw_When_Total_Amount_Is_Null()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new FiscalDocumentItem(
                1,
                "PROD001",
                "Produto teste",
                "12345678",
                new CFOP("5102"),
                2,
                new Money(50),
                null!));

        Assert.That(exception!.Message, Is.EqualTo("O valor total do item não foi informado."));
    }

    [Test(Description = "Deve iniciar os tributos do item zerados")]
    public void Should_Initialize_Item_Taxes_With_Zero_Values()
    {
        var item = CreateItem();

        Assert.Multiple(() =>
        {
            Assert.That(item.Taxes.ICMS, Is.EqualTo(Money.Zero));
            Assert.That(item.Taxes.IPI, Is.EqualTo(Money.Zero));
            Assert.That(item.Taxes.PIS, Is.EqualTo(Money.Zero));
            Assert.That(item.Taxes.COFINS, Is.EqualTo(Money.Zero));
            Assert.That(item.Taxes.IBS, Is.EqualTo(Money.Zero));
            Assert.That(item.Taxes.CBS, Is.EqualTo(Money.Zero));
        });
    }

    [Test(Description = "Deve definir os tributos do item")]
    public void Should_Set_Item_Taxes()
    {
        var item = CreateItem();

        var taxes = new TaxSummary
        {
            ICMS = new Money(18),
            IPI = new Money(10),
            PIS = new Money(1.65m),
            COFINS = new Money(7.60m),
            IBS = new Money(5),
            CBS = new Money(8)
        };

        item.SetTaxes(taxes);

        Assert.That(item.Taxes, Is.EqualTo(taxes), "Os tributos do item não foram definidos corretamente.");
    }

    [Test(Description = "Não deve permitir tributos nulos no item")]
    public void Should_Throw_When_Item_Taxes_Are_Null()
    {
        var item = CreateItem();

        var exception = Assert.Throws<DomainException>(() => item.SetTaxes(null!));

        Assert.That(exception!.Message, Is.EqualTo("Os tributos do item não foram informados."));
    }

    private static FiscalDocumentItem CreateItem()
    {
        return new FiscalDocumentItem(
            1,
            "PROD001",
            "Produto teste",
            "12345678",
            new CFOP("5102"),
            2,
            new Money(50),
            new Money(100));
    }
}