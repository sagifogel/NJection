using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;
using NJection.Extensions;

namespace NJection.Expressions
{
    public class ListInitExpressionInjector : GenericInjector<ListInitExpression>
    {
        private const string _expression = "expression";
        private const string _initializer = "initializer";
        private const string _initializers = "initializers";

        public ListInitExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.ListInit; }
        }

        public override ListInitExpression Parse()
        {
            NewExpression expression = null;
            IEnumerable<ElementInit> elementInits = null;
            XElement initializers = this.ConfigurationElement.Element(_initializers);
            XElement expressionElement = this.ConfigurationElement.Element(_expression);

            if (expressionElement == null)
                throw new ArgumentNullException(_expression);    
            
            if (!TryResolve(expressionElement, out expression))
                throw new ArgumentTypeException(typeof(NewExpression), _expression);

            if (initializers != null && initializers.HasElements)
            {
                elementInits = from initElement in initializers.Elements(_initializer)
                               select new ElementInitInjector(this, this.ParentBlock, initElement, expression.Type).Value;
            }

            return Expression.ListInit(expression, elementInits);
        }

        private bool TryResolve(XElement expressionElement, out NewExpression newExpression)
        {
            newExpression = this.Resolve<NewExpression>(expressionElement);
            return newExpression != null;
        }
    }
}
