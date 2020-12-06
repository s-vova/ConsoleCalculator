using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class TokenTests
    {
        [Fact]
        public void CanCreateNumberToken()
        {
            Token token = new Token(2.234);
            
            Assert.Equal(Token.NUMBER_TOKEN, token.Type);
            Assert.Equal(2.234, token.Value);
        }

        [Fact]
        public void CanCreateNonNumberToken()
        {
            Token token = new Token("+");

            Assert.Equal("+", token.Type);
        }

        [Fact]
        public void TestTokenIsNumberPropTrue()
        {
            Token token = new Token(34);
            Assert.True(token.IsNumber);
        }
        [Fact]
        public void TestTokenIsNumberPropFalse()
        {
            Token token = new Token("*");
            Assert.False(token.IsNumber);
        }
    }
}
