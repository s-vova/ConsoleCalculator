using ConsoleCalculator.Domain;
using System.Collections.Generic;

namespace ConsoleCalculator.Interfaces
{
    public interface ICalculator
    {
        double Compute(IList<Token> tokensList, IDictionary<string, Operator> operators);
    }
}