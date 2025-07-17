using Xunit;
using System;

public class CalculatorTests
{
    [Fact]
    public void Add_ShouldReturnCorrectSum()
    {
        dynamic calculator = new Calc().CreateCalculator();

        int result = calculator.Add(220, 8);

        Assert.Equal(228, result);
    }

    [Fact]
    public void Minus_ShouldReturnCorrectDifference()
    {
        dynamic calculator = new Calc().CreateCalculator();

        int result = calculator.Minus(6, -3);

        Assert.Equal(9, result);
    }

    [Fact]
    public void Mul_ShouldReturnCorrectProduct()
    {
        dynamic calculator = new Calc().CreateCalculator();

        int result = calculator.Mul(2, 5);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Div_ShouldReturnCorrectQuotient()
    {
        dynamic calculator = new Calc().CreateCalculator();

        int result = calculator.Div(15, 3);

        Assert.Equal(5, result);
    }
}