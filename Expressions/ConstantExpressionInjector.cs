using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class ConstantExpressionInjector : GenericInjector<ConstantExpression>
    {
        private const string _value = "value";

        public override ExpressionType Type
        {
            get { return ExpressionType.Constant; }
        }

        public ConstantExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ConstantExpression Parse()
        {
            Type type;
            string value;
            ConstantExpression expression = null;
            bool isValueNullOrWhiteSpace = false;
            XAttribute nameAttribute = ConfigurationElement.Attribute(_name);
            XAttribute typeAttribute = ConfigurationElement.Attribute(_typeOf);

            if (!TryGetAttributeValue(this.ConfigurationElement, out value, _value))
                isValueNullOrWhiteSpace = true;
            
            if (TryGetType(typeAttribute.Value, out type))
            {
                ConstructorInfo ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis, new Type[] { }, null);

                if (isValueNullOrWhiteSpace && ctor != null)
                    expression = Expression.Constant(Activator.CreateInstance(type), type);
                else
                    expression = Expression.Constant(Convert.ChangeType(value, type), type);
            }
            else
            {
                if (isValueNullOrWhiteSpace)
                    throw new ArgumentNullException(value);

                expression = Expression.Constant(value);
            }

            return expression;
        }
    }
}
