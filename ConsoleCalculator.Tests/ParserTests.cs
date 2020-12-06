using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class ParserTests
    {
        static readonly string[] OPERATORS_LIST = { "+", "-", "*", "/", "(", ")" };
        
        string TokensJoin(IList<Token> tokens)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in tokens)
            {
                if (item.IsNumber)
                {
                    result.Append(item.Value);
                }
                else
                {
                    result.Append(item.Type);
                }
            }

            return result.Replace(",", ".").ToString();
        }

        [Theory]
        [InlineData("2.5+6*7-9")]
        [InlineData("2.5 +6  * 7 - 9")]
        [InlineData("(2.5 +6)  * 7 - 9+5.2-9.4/7.456")]
        public void ParseTests(string s)
        {
            s = s.Replace(" ", "");
            Assert.Equal(s, TokensJoin(Parser.Parse(s, OPERATORS_LIST)));
        }

        [Theory]
        [InlineData("1+2-3q")]
        [InlineData("1+2-3+q")]
        public void ParserInvalidSymbol(string s)
        {
            Assert.Throws<Exception>(() => {Parser.Parse(s, OPERATORS_LIST); });
        }
    }
}
