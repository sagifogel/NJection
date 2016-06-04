using System;
using System.Runtime.Serialization;

namespace NJection.Exceptions
{
    public class InvalidExpressionException : ArgumentException
    {
        public InvalidExpressionException() { }

        public InvalidExpressionException(string expressionName)
            : base(string.Format("Expression name: {0}.", expressionName)) { }

        protected InvalidExpressionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public InvalidExpressionException(string message, Exception innerException)
            : base(message, innerException) { }

        public InvalidExpressionException(string expressionName, string message)
            : base(string.Format("{0}.{1}Expression name: {2}.", message, Environment.NewLine, expressionName)) { }
    }
}
