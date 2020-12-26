using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ConsoleCalculator.Interfaces;
using System.Threading;

namespace ConsoleCalculator
{
    public class UI
    {
        private readonly TextReader Input;
        private readonly TextWriter Out;
        private readonly TextWriter OutErr;

        public UI( TextWriter Out, TextReader Input, TextWriter OutErr)
        {
            this.Input = Input;
            this.Out = Out;
            this.OutErr = OutErr;
        }

        public void Run(ISolver solver)
        {
            Out.WriteLine("Welcome to Calculator");
            Out.WriteLine("To close the program, enter empty string");
            Out.WriteLine();

            while (true)
            {
                Out.Write("Enter expression: ");
                string expression = Input.ReadLine();

                if (string.IsNullOrWhiteSpace(expression))
                {
                    break;
                }

                double result = solver.Solve(expression);
                Out.WriteLine($"Result: {result}");
            }
            Out.WriteLine("Goodbye!");
        }
    }
}

