using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Domain
{
    public class Token
    {
        public const string NUMBER_TOKEN = "NUMBER";

        public string Type;
        public double Value;

        public Token(string type, double value = 0)
        {
            Type = type;
            Value = value;
        }

        public Token(double value) : this(Token.NUMBER_TOKEN, value)
        {
        }

        public bool IsNumber
        {
            get
            {
                return Type == NUMBER_TOKEN;
            }
        }

        public override string ToString()
        {
            return $"Token(\"{Type}\", {Value})";
        }

    }
}
