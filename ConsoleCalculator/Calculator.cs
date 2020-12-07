using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    public class Calculator
    {
        Stack<Token> Tokens = new Stack<Token>();
        IDictionary<string, Operator> Operators;

        public Calculator(IDictionary<string, Operator> operators)
        {
            Operators = operators;
        }

        public double Compute(IList<Token> tokensList)
        {
            Tokens.Clear();
            for (int i = tokensList.Count - 1; i >= 0; i--)
            {
                Tokens.Push(tokensList[i]);
            }

            return Calculate(Tokens.Pop());
        }

        double Calculate(Token leftValue)
        {
            if (leftValue.Type == "(")
            {
                if (Tokens.TryPop(out Token t))
                {
                    leftValue.Type = Token.NUMBER_TOKEN;
                    leftValue.Value = Calculate(t);
                }
                else
                {
                    throw new InvalidSyntaxException("Invalid Syntax");
                }
                if (!Tokens.TryPop(out _))
                {
                    throw new InvalidSyntaxException("\")\" not found");
                }
            }

            Token op;
            if (!Tokens.TryPeek(out op) || op.Type == ")")
            {
                // если нет оператора или ) значит конец выражения
                return leftValue.Value;
            }

            op = Tokens.Pop();
            if (op.IsNumber)
            {
                throw new InvalidSyntaxException("Invalid syntax");
            }

            Token rightValue;
            if (!Tokens.TryPop(out rightValue))
            {
                // если есть оператор op (см выше) но нет операнда rval значит синтаксическая ошибка 
                throw new InvalidSyntaxException($"SyntaxError after operator \"{op.Type}\" must be operand!");
            }

            if (rightValue.Type == "(")
            {
                rightValue.Value = Calculate(Tokens.Pop());
                rightValue.Type = Token.NUMBER_TOKEN;
                if (!Tokens.TryPop(out _))
                {
                    throw new InvalidSyntaxException("\")\" not found");
                }
            }

            if (!rightValue.IsNumber)
            {
                throw new InvalidSyntaxException("Invalid syntax");
            }

            // PeekOp - это следующий оператор
            Token peekOp;
            if (Tokens.TryPeek(out peekOp))
            {
                if (peekOp.IsNumber)
                {
                    throw new InvalidSyntaxException("Invalid Syntax");
                }
                if (Operators[peekOp.Type].Priority > Operators[op.Type].Priority)
                {
                    rightValue.Value = Calculate(rightValue);
                }
            }

            leftValue.Value = Operators[op.Type].Do(leftValue.Value, rightValue.Value);

            if (peekOp == null)
            {
                // если дальше ничего нет
                return leftValue.Value;
            }

            return Calculate(leftValue);
        }
    }
}
