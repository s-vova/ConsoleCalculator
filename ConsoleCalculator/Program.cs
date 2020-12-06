using System;
using System.Linq;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string expression = Console.ReadLine();
            var tokens = Parser.Parse(expression, new string[] { "+", "-", "*", "/" });
            Console.WriteLine(string.Join(",\n", tokens));
        }
    }
}
