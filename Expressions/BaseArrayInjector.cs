using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public abstract class BaseArrayInjector<T> : GenericInjector<T> where T : Expression
    {
        private const string _indexes = "indexes";
        private const string _expression = "expression";

        public override abstract ExpressionType Type { get; }
        protected abstract T CreateExpression(Expression arrayExpression, IEnumerable<Expression> indexes);

        public BaseArrayInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }
        
        public override T Parse()
        {
            Expression expression = null;
            IEnumerable<Expression> indexes = null;
            XElement indexesElement = this.ConfigurationElement.Element(_indexes);
            XAttribute typeAttribute = this.ConfigurationElement.Attribute(_type);

            if (typeAttribute == null)
                throw new InvalidExpressionException();

            if (!this.ConfigurationElement.HasElements)
                throw new ArgumentNullException(typeAttribute.Value);

            if (indexesElement == null || !indexesElement.HasElements)
                throw new ArgumentNullException(_indexes);

            indexes = from indeconfigurationElement in indexesElement.Elements(_expression)
                      select this.Resolve<Expression>(indeconfigurationElement);

            expression = this.Resolve<Expression>(this.ConfigurationElement.FirstNode as XElement);

            if (!expression.Type.IsArray)
                throw new ArgumentTypeException(typeof(Array), expression.NodeType.ToString());
            
            return CreateExpression(expression, indexes);
        }
    }
}
