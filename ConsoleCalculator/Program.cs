using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Calculator");
            Console.WriteLine($"Allowed operators: {string.Join(" ", Operator.OperatorsDict.Keys)}");
            Console.WriteLine("To close the program, enter empty string");
            Console.WriteLine();

            Calculator calculator = new Calculator(Operator.OperatorsDict);

            while (true)
            {
                Console.Write("Enter expression: ");
                string expression = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(expression))
                {
                    break;
                }

                try
                {
                    IList<Token> tokensList = Parser.Parse(expression, Operator.OperatorsDict.Keys);

                    double result = calculator.Compute(tokensList);
                    Console.WriteLine($"Result: {result}");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Zero division error occured!");
                }
                catch (InvalidSyntaxException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            Console.WriteLine("Goodbye!");
        }
    }
}
