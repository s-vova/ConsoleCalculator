using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Domain
{
    // Представляет математический оператор как функцию и приоритет
    public class Operator
    {
        public Func<double, double, double> Do;
        public int Priority;

        public Operator(Func<double, double, double> action, int priority)
        {
            Do = action;
            Priority = priority;
        }
    }
}

