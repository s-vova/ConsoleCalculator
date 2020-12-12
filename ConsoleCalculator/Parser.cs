using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using ConsoleCalculator.Domain;

namespace ConsoleCalculator
{
    static public class Parser
    {
        static public IList<Token> Parse(string expression, IEnumerable<string> operatorsList)
        {
            expression = expression.Replace(",", ".");

            var result = new List<Token>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    continue;
                }

                if (char.IsNumber(expression[i]))
                {
                    var numberStr = new StringBuilder();

                    while (i < expression.Length && (char.IsNumber(expression[i]) || expression[i] == '.'))
                    {
                        numberStr.Append(expression[i]);
                        i++;
                    }

                    double value = Double.Parse(numberStr.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                    i--;
                    result.Add(new Token(Token.NUMBER_TOKEN, value));
                    continue;
                }

                else if (operatorsList.Contains(expression[i].ToString()) || expression[i] == '(' || expression[i] == ')')
                {
                    result.Add(new Token(expression[i].ToString()));
                }

                else
                {
                    throw new InvalidSyntaxException($"Invalid symbol \"{expression[i]}\" in {i} position");
                }
            }
            return result;
        }
    }
}
