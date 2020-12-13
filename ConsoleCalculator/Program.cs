﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Domain;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main()
        {
            // Словарь, в котором задается: оператор, функция которую он выполняет, и его приоритет
            Dictionary<string, Operator> OperatorsDict = new Dictionary<string, Operator>
            {
                { "+", new Operator((a,b) => a+b, priority : 1) },
                { "-", new Operator((a,b) => a-b, priority : 1) },
                { "*", new Operator((a,b) => a*b, priority : 2) },
                { "/", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a/b, priority : 2) },
                // Пример добавления новго оператора, обратите внимание, что приоритет возведения в степень выше, чем у * и /
                // { "^", new Operator((a,b) => Math.Pow(a,b), priority : 3) }, 
                // Пример добавления нового оператора
                // { "%", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a%b, priority : 2) },  
            };

            Console.WriteLine("Welcome to Calculator");
            Console.WriteLine($"Allowed operators: {string.Join(" ", OperatorsDict.Keys)}");
            Console.WriteLine("To close the program, enter empty string");
            Console.WriteLine();

            Calculator calculator = new Calculator(OperatorsDict);

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
                    IList<Token> tokensList = Parser.Parse(expression, OperatorsDict.Keys);

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
