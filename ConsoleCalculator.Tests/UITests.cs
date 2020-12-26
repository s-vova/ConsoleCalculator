using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.IO;
using ConsoleCalculator.Interfaces;
using ConsoleCalculator;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class UITests
    {
        // Этот тест сделан асинхронным, потому что в xUnit есть баг, 
        // из-за которого Timeout работает только если метод асинхронный 
        // ссылка https://github.com/xunit/xunit/issues/217#issuecomment-625402054
        // таймаут нужен потому что есть вероятность попадания в бесконечный цикл
        [Fact(Timeout = 1000)]
        public async Task UIPrintsResultFromSolver()
        {
            await Task.Run(() =>
            {
                string expressions =
                   "1 + 1\n" +
                   "2 + 2\n" +
                   "\n";
                StringReader stringIn = new StringReader(expressions);

                StringWriter stringOut = new StringWriter();
                StringWriter stringErr = new StringWriter();

                var mock = new Mock<ISolver>();
                mock
                    .SetupSequence(m => m.Solve(It.IsAny<string>()))
                    .Returns(2)
                    .Returns(4)
                    .Throws(new InvalidOperationException());

                UI ui = new UI(stringOut, stringIn, stringErr);

                // Act
                ui.Run(mock.Object);

                // Assert
                Assert.Contains("Result: 2", stringOut.ToString());
                Assert.Contains("Result: 4", stringOut.ToString());
            });
           
        }
    }
}
