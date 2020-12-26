using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    [Serializable]
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException() : base("Syntax error") { }
        public InvalidSyntaxException(string message) : base(message) { }
        public InvalidSyntaxException(string message, Exception inner) : base(message, inner) { }
        protected InvalidSyntaxException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
