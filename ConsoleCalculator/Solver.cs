using System;
using System.Collections.Generic;
using System.Text;
using ConsoleCalculator.Domain;
using ConsoleCalculator.Interfaces;

namespace ConsoleCalculator
{
    public class Solver : ISolver
    {
        // Словарь, в котором задается: оператор, функция которую он выполняет, и его приоритет
        protected Dictionary<string, Operator> Operators = new Dictionary<string, Operator>
            {
                { "+", new Operator((a,b) => a+b, priority : 1) },
                { "-", new Operator((a,b) => a-b, priority : 1) },
                { "*", new Operator((a,b) => a*b, priority : 2) },
                { "/", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a/b, priority : 2) },
            };
        public IEnumerable<string> AllowedOperators { get => Operators.Keys; }

        readonly ICalculator Calculator;
        readonly Func<string, IEnumerable<string>, IList<Token>> Parse;

        public Solver(ICalculator calculator, Func<string, IEnumerable<string>, IList<Token>> parse)
        {
            Calculator = calculator;
            Parse = parse;
        }

        public double Solve(string expression)
        {
            IList<Token> tokens = Parse(expression, AllowedOperators);
            double result = Calculator.Compute(tokens, Operators);
            return result;
        }
    }
}
