using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class CalculatorTests
    {
        // Словарь, в котором задается: оператор, функция которую он выполняет, и его приоритет
        static readonly Dictionary<string, Operator> OperatorsDict = new Dictionary<string, Operator>
        {
            { "+", new Operator((a,b) => a+b, priority : 1) },
            { "-", new Operator((a,b) => a-b, priority : 1) },
            { "*", new Operator((a,b) => a*b, priority : 2) },
            { "/", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a/b, priority : 2) },
            { "(", new Operator((a,b) => throw new Exception("Shouldn't be called"), priority : 10) },
            { ")", new Operator((a,b) => throw new Exception("Shouldn't be called"), priority : 10) },
        };

        [Theory]
        [InlineData("1+1", 2)]
        [InlineData("1+2", 3)]
        [InlineData("2*5.1", 10.2)]
        [InlineData("2/5", 0.4)]
        [InlineData("1+1+1", 3)]
        [InlineData("3-2-1", 0)]
        [InlineData("10+2*5*2*5*3*7", 2110)]
        [InlineData("5,5/2*24,21-5+3", 64.5775)]
        [InlineData("5,5/2+3", 5.75)]
        [InlineData("50/5/2", 5)]
        [InlineData("2*2*3", 12)]
        [InlineData("2*2+3", 7)]
        [InlineData("1-2-3", -4)]
        [InlineData("10-3-6", 1)]
        [InlineData("3-2+2", 3)]
        [InlineData("1+2+3", 6)]
        [InlineData("(1-2)-6", -7)]
        [InlineData("10-(5-2)", 7)]
        [InlineData("10*(5-2)", 30)]
        [InlineData("(2+3)-(4+3)", -2)]
        [InlineData("(2+3)-(4+(3*(2-3)))", 4)]
        [InlineData("(2+3)*(6-8)", -10)]
        [InlineData("-5+5", 0)]
        [InlineData("-(5+5)", -10)]
        [InlineData("(-5)*5", -25)]
        [InlineData("5*(-5)", -25)]
        [InlineData("(-2)*(-3)", 6)]
        [InlineData("-(-(2-3))", -1)]
        public void SimpleTests(string e, double r)
        {
            IList<Token> tokensList = Parser.Parse(e,OperatorsDict.Keys);
            Calculator calculator = new Calculator(OperatorsDict);
            Assert.Equal(r, calculator.Compute(tokensList));
        }

        [Theory]
        [InlineData("1+a")]
        [InlineData("1+5+")]
        [InlineData("1+5+5*")]
        [InlineData("1+5+5 1")]
        [InlineData("1+5*(2+3")]
        [InlineData("1+5*(2+3+(5*)")]
        [InlineData("--5")]
        [InlineData("(3+3)--(2-3)")]
        [InlineData("-3*-2")]
        public void ShoudldThrowInvalidSyntax(string e)
        {
            Calculator calculator = new Calculator(OperatorsDict);
            Assert.Throws<InvalidSyntaxException>(() => calculator.Compute(Parser.Parse(e, OperatorsDict.Keys)));
        }

        [Theory]
        [InlineData("5/0")]
        [InlineData("5/(2-2)")]
        [InlineData("5/(2*0)")]
        public void ShouldThrowZeroDivision(string e)
        {
            Calculator calculator = new Calculator(OperatorsDict);
            Assert.Throws<DivideByZeroException>(() => calculator.Compute(Parser.Parse(e, OperatorsDict.Keys)));
        }
    }
}
