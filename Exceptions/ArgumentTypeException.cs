using System;
using System.Runtime.Serialization;

namespace NJection.Exceptions
{
    public class ArgumentTypeException : ArgumentException
    {
        public ArgumentTypeException(Type type, string argumentName) 
            : base(string.Format("Expected type {0}.{1}Argument name: {2}.", type.Name, Environment.NewLine, argumentName)) { }

        public ArgumentTypeException(string message)
            : base(message) { }

        protected ArgumentTypeException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public ArgumentTypeException(string message, Exception innerException) 
            : base(message, innerException) { }

        public ArgumentTypeException(string argumentName, string message)
            : base(string.Format("{0}.{1}Argument name: {2}.", message, Environment.NewLine, argumentName)) { }

        public ArgumentTypeException(Type type, string argumentName, string message)
            : base(string.Format("Expected type {0}.{1}Argument name: {2}.", type.Name, Environment.NewLine, argumentName), new ArgumentException(message)) { }
    }
}
