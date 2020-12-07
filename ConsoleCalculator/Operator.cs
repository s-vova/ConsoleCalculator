﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    // Класс хранящий делегат оператора и приоритет оператора
    public class Operator
    {
        public Func<double, double, double> Do;
        public int Priority;

        public Operator(Func<double, double, double> func, int p)
        {
            Do = func;
            Priority = p;
        }

        // Глобальный словарь объединяющий оператор, функцию которую он выполняет и его приоритет
        public static Dictionary<string, Operator> OperatorsDict = new Dictionary<string, Operator>
        {
            { "+", new Operator((a,b) => a+b, 1) },
            { "-", new Operator((a,b) => a-b, 1) },
            { "*", new Operator((a,b) => a*b, 2) },
            { "/", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a/b, 2) },
            { "(", new Operator((a,b) => throw new Exception("Shouldn't be called"), 10) },
            { ")", new Operator((a,b) => throw new Exception("Shouldn't be called"), 10) },
            //{ "^", new Operator((a,b) => Math.Pow(a,b), 3) }, // Пример добавления новго оператора
            //{ "%", new Operator((a,b) => b==0 ? throw new DivideByZeroException() : a%b, 2) },  // Пример добавления нового оператора
        };
    }
}
