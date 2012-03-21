using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public abstract class BaseTypeBinaryExpression : GenericInjector<TypeBinaryExpression>
    {
        private const string _expression = "expression";

        public override abstract ExpressionType Type { get; }
        protected abstract TypeBinaryExpression CreateTypeBinaryExpression(Expression expression, Type type);

        public BaseTypeBinaryExpression(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override TypeBinaryExpression Parse()
        {
            Type type;
            Expression expression = null;
            XElement expressionElement = this.ConfigurationElement.Element(_expression);

            TryGetType(this.ConfigurationElement, out type);

            if (expressionElement == null)
                throw new ArgumentNullException(this.Type.ToString());

            expression = this.Resolve<Expression>(expressionElement);

            return CreateTypeBinaryExpression(expression, type);
        }
    }
}
