using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    public class Token
    {
        public const string NUMBER_TOKEN = "NUMBER";

        public string Type;
        public double Value;

        public Token(string t, double v = 0)
        {
            Type = t;
            Value = v;
        }

        public Token(double v) : this(Token.NUMBER_TOKEN, v)
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
