using System;
using System.Runtime.Serialization;

namespace NJection.Exceptions
{
    public class AttributeNullException : ArgumentException
    {
        public AttributeNullException(string attributeName)
            : base(string.Format("Attribute is null or could not be resolved.{0}Attribute name: {1}.", Environment.NewLine, attributeName)) { }

        protected AttributeNullException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public AttributeNullException(string message, Exception innerException) 
            : base(message, innerException) { }

        public AttributeNullException(string attributeName, string message)
            : base(string.Format("{0}.{1}Attribute name: {2}.", message, Environment.NewLine, attributeName)) { }
    }
}
