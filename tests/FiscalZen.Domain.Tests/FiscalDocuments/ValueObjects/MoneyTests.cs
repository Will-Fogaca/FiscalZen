using FiscalZen.Domain.FiscalDocuments.ValueObjects;

namespace FiscalZen.Domain.Tests;

public class MoneyTests
{
    [Test(Description = "Deve somar dois valores monetários")]
    public void Should_Add_Two_Money_Values()
    {
        var first = new Money(100);
        var second = new Money(50);

        var result = first + second;

        Assert.That(result.Value, Is.EqualTo(150), "A soma dos valores monetários está incorreta.");
    }

    [Test(Description = "Deve subtrair dois valores monetários")]
    public void Should_Subtract_Two_Money_Values()
    {
        var first = new Money(100);
        var second = new Money(40);

        var result = first - second;

        Assert.That(result.Value, Is.EqualTo(60), "A subtração dos valores monetários está incorreta.");
    }

    [Test(Description = "Deve multiplicar um valor monetário por um número")]
    public void Should_Multiply_Money_By_Number()
    {
        var money = new Money(100);

        var result = money * 3;

        Assert.That(result.Value, Is.EqualTo(300), "A multiplicação do valor monetário está incorreta.");
    }

    [Test(Description = "Deve dividir um valor monetário por um número")]
    public void Should_Divide_Money_By_Number()
    {
        var money = new Money(100);

        var result = money / 4;

        Assert.That(result.Value, Is.EqualTo(25), "A divisão do valor monetário está incorreta.");
    }

    [Test(Description = "Deve identificar quando o primeiro valor monetário é maior que o segundo")]
    public void Should_Return_True_When_First_Money_Is_Greater()
    {
        var first = new Money(100);
        var second = new Money(50);

        var result = first > second;

        Assert.That(result, Is.True, "O primeiro valor deveria ser maior que o segundo.");
    }

    [Test(Description = "Deve identificar quando o primeiro valor monetário é menor que o segundo")]
    public void Should_Return_True_When_First_Money_Is_Less()
    {
        var first = new Money(50);
        var second = new Money(100);

        var result = first < second;

        Assert.That(result, Is.True, "O primeiro valor deveria ser menor que o segundo.");
    }

    [Test(Description = "Deve identificar quando um valor monetário é maior ou igual a outro")]
    public void Should_Return_True_When_Money_Is_Greater_Or_Equal()
    {
        var first = new Money(100);
        var second = new Money(100);

        var result = first >= second;

        Assert.That(result, Is.True, "Os valores iguais deveriam satisfazer a comparação de maior ou igual.");
    }

    [Test(Description = "Deve identificar quando um valor monetário é menor ou igual a outro")]
    public void Should_Return_True_When_Money_Is_Less_Or_Equal()
    {
        var first = new Money(100);
        var second = new Money(100);

        var result = first <= second;

        Assert.That(result, Is.True, "Os valores iguais deveriam satisfazer a comparação de menor ou igual.");
    }
}