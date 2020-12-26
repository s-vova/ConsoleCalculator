using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using ConsoleCalculator.Interfaces;
using ConsoleCalculator.Domain;

namespace ConsoleCalculator.Tests
{
    public class SolverTests
    {
        [Fact]
        public void SolverReturnsResultOfExpresion()
        {
            Token[] tokens = new Token[] {
                    new Token(1),
                    new Token("+"),
                    new Token(1),
                };

            Mock<Func<string, IEnumerable<string>, IList<Token>>> parserMock = new Mock<Func<string, IEnumerable<string>, IList<Token>>>();
            parserMock
                .Setup(parse => parse(It.Is<string>(m => m == "1+1"), It.IsAny<IEnumerable<string>>()))
                .Returns(tokens);


            var calcMock = new Mock<ICalculator>();
            calcMock
                .Setup(x => x.Compute(It.Is<IList<Token>>(t => t == tokens), It.IsAny<IDictionary<string, Operator>>()))
                .Returns(2);

            Solver solver = new Solver(calcMock.Object, parserMock.Object);

            // Act
            double actualResult = solver.Solve("1+1");

            // Assert
            parserMock.Verify(parse => parse("1+1", It.IsAny<IEnumerable<string>>()), Times.AtLeastOnce);
            calcMock.Verify(x => x.Compute(tokens, It.IsAny<IDictionary<string, Operator>>()), Times.AtLeastOnce);
            Assert.Equal(2, actualResult);
        }
    }
}
