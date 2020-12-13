using System;
using System.Collections.Generic;
using System.Text;
using ConsoleCalculator.Domain;

namespace ConsoleCalculator
{
    public class Calculator
    {
        Stack<Token> Tokens = new Stack<Token>();
        readonly IDictionary<string, Operator> Operators;

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
        // Рекурсивно обрабатывает выражение слева направо, токены хранятся в стэке Tokens. На верху стэка хранится левый токен
        double Calculate(Token leftValue)
        {
            if (leftValue.Type == "-")
            {
                Tokens.Push(leftValue);
                leftValue = new Token(0);
            }

            if (leftValue.Type == "(")
            {
                if (Tokens.TryPop(out Token t))
                {
                    leftValue = new Token(Calculate(t));
                }
                else
                {
                    throw new InvalidSyntaxException();
                }
                if (!Tokens.TryPop(out _))
                {
                    throw new InvalidSyntaxException("Syntax error \")\" not found");
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
                throw new InvalidSyntaxException();
            }

            Token rightValue;
            if (!Tokens.TryPop(out rightValue))
            {
                // если есть оператор op но нет операнда rightValue значит синтаксическая ошибка 
                throw new InvalidSyntaxException();
            }

            if (rightValue.Type == "(")
            {
                rightValue = new Token(Calculate(Tokens.Pop()));
                if (!Tokens.TryPop(out _))
                {
                    throw new InvalidSyntaxException("Syntax error \")\" not found");
                }
            }

            if (!rightValue.IsNumber)
            {
                throw new InvalidSyntaxException();
            }

            // PeekOp - это следующий оператор
            Token peekOp;
            if (Tokens.TryPeek(out peekOp))
            {
                if (peekOp.IsNumber)
                {
                    throw new InvalidSyntaxException();
                }
                if (Operators[peekOp.Type].Priority > Operators[op.Type].Priority)
                {
                    rightValue = new Token(Calculate(rightValue));
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
