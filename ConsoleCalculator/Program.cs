using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Domain;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main()
        {
            Calculator calculator = new Calculator();
            Solver solver = new Solver(calculator, Parser.Parse);
            UI ui = new UI(Console.Out, Console.In, Console.Error);

            ui.Run(solver, solver.AllowedOperators);
        }
    }
}
