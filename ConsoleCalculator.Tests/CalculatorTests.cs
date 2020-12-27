using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ConsoleCalculator.Domain;

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
        };

        private void RunTest(double expectedResult, IList<Token> tokens)
        {
            Calculator calculator = new Calculator();

            double actualResult = calculator.Compute(tokens, OperatorsDict);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(CanCalculateSimpleExpressionsData))]
        public void CanCalculateSimpleExpressions(double expectedResult, Token[] tokens)
        {
            RunTest(expectedResult, tokens);
        }

        public static IEnumerable<object[]> CanCalculateSimpleExpressionsData =>
            new List<object[]>
            {
                new object[]
                {
                    1d + 1d,
                    new Token[]
                    {
                        new Token(1),
                        new Token("+"),
                        new Token(1),
                    }
                },
                new object[]
                {
                    5d - 2d * 3d,
                    new Token[]
                    {
                        new Token(5),
                        new Token("-"),
                        new Token(2),
                        new Token("*"),
                        new Token(3),
                    }
                },
                new object[]
                {
                    5d * 2d - 3d / 2d,
                    new Token[]
                    {
                        new Token(5),
                        new Token("*"),
                        new Token(2),
                        new Token("-"),
                        new Token(3),
                        new Token("/"),
                        new Token(2),
                    }
                },
                new object[]
                {
                    5d + 2d * 3d - 2d,
                    new Token[]
                    {
                        new Token(5),
                        new Token("+"),
                        new Token(2),
                        new Token("*"),
                        new Token(3),
                        new Token("-"),
                        new Token(2),
                    }
                },
                new object[]
                {
                    5d - 2d * 3d * 7d,
                    new Token[]
                    {
                        new Token(5),
                        new Token("-"),
                        new Token(2),
                        new Token("*"),
                        new Token(3),
                        new Token("*"),
                        new Token(7),
                    }
                },
                new object[]
                {
                    -10d - 2.5d * 3d * -75d,
                    new Token[]
                    {
                        new Token(-10),
                        new Token("-"),
                        new Token(2.5),
                        new Token("*"),
                        new Token(3),
                        new Token("*"),
                        new Token(-75),
                    }
                },
            };

        [Theory]
        [MemberData(nameof(CanCalculateSimpleExpressionsData))]
        public void CanCalculateWithBrackets(double exptectedResult, Token[] tokens)
        {
            RunTest(exptectedResult, tokens);
        }

        public static IEnumerable<object[]> CanCalculateWithBracketsData =>
            new List<object[]>
            {
                new object[]
                {
                    2d * (4d - 5d),
                    new Token[]
                    {
                        new Token(2),
                        new Token("*"),
                        new Token("("),
                        new Token(4),
                        new Token("-"),
                        new Token(5),
                        new Token(")"),
                    }
                },
                new object[]
                {
                    2d * (4d * 5d),
                    new Token[]
                    {
                        new Token(2),
                        new Token("*"),
                        new Token("("),
                        new Token(4),
                        new Token("*"),
                        new Token(5),
                        new Token(")"),
                    }
                },
                new object[]
                {
                    (2d - 3d) * (5d + 12d),
                    new Token[]
                    {
                        new Token("("),
                        new Token(2),
                        new Token("-"),
                        new Token(3),
                        new Token(")"),
                        new Token("*"),
                        new Token("("),
                        new Token(5),
                        new Token("+"),
                        new Token(12),
                        new Token(")"),
                    }
                },
                new object[]
                {
                    -(-(2d-3d)),
                    new Token[]
                    {
                        new Token("-"),
                        new Token("("),
                        new Token("-"),
                        new Token("("),
                        new Token(2),
                        new Token("-"),
                        new Token(3),
                        new Token(")"),
                        new Token(")"),                        
                    }
                },
            };
    
        [Theory]
        [MemberData(nameof(ThrowsInvalidSyntaxData))]
        public void ThrowsInvalidSyntax(Token[] tokens)
        {
            Calculator calculator = new Calculator();

            Assert.Throws<InvalidSyntaxException>(()=> 
            { 
                calculator.Compute(tokens, OperatorsDict); 
            });
        }

        public static IEnumerable<object[]> ThrowsInvalidSyntaxData =>
           new List<object[]>
           {
                new object[]
                {
                    new Token[]
                    {
                        new Token(2),
                        new Token("*"),
                    },
                },
                new object[]
                {
                    new Token[]
                    {
                        new Token(2),
                        new Token("*"),
                        new Token(2),
                        new Token("+"),
                    },
                },
                new object[]
                {
                    new Token[]
                    {
                        new Token("-"),
                        new Token("("),
                        new Token("("),
                        new Token("-"),
                        new Token("("),
                        new Token(2),
                        new Token("-"),
                        new Token(3),
                        new Token(")"),
                        new Token(")"),
                    },
                }
           };

        [Theory]
        [MemberData(nameof(ThrowsZeroDivisionErrorData))]
        public void ThrowsZeroDivisionError(Token[] tokens)
        {
            Calculator calculator = new Calculator();

            Assert.Throws<DivideByZeroException>(() =>
            {
                calculator.Compute(tokens, OperatorsDict);
            });
        }

        public static IEnumerable<object[]> ThrowsZeroDivisionErrorData =>
           new List<object[]>
           {
                new object[]
                {
                    new Token[]
                    {
                        new Token(2),
                        new Token("/"),
                        new Token(0),
                    },
                },
                new object[]
                {
                    new Token[]
                    {
                        new Token(2),
                        new Token("/"),
                        new Token("("),
                        new Token(5),
                        new Token("-"),
                        new Token(5),
                        new Token(")"),
                    },
                },

           };
    }
}
