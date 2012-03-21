using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    class InvocationExpressionInjector :  GenericInjector<InvocationExpression>
    {
        private const string _arguments = "arguments"; 
        private const string _expression = "expression";

        public override ExpressionType Type
        {
            get { return ExpressionType.Invoke; }
        }
        
        public InvocationExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override InvocationExpression Parse()
        {
            LambdaExpression expression = null;
            IEnumerable<Expression> arguments = null;
            XElement lambdaElement = this.ConfigurationElement.Element(_expression);
            XElement argumentsElement = this.ConfigurationElement.Element(_arguments);

            if (lambdaElement == null)
                throw new ArgumentNullException(_expression);

            if (ResolveType(lambdaElement) != ExpressionType.Lambda)
                throw new ArgumentTypeException(typeof(LambdaExpression), _expression);

            if (argumentsElement == null || !argumentsElement.HasElements)
                throw new ArgumentNullException(_arguments);
                
            arguments = from arguemntElement in argumentsElement.Elements(_expression)
                        select this.Resolve<Expression>(arguemntElement);

            expression = this.Resolve<LambdaExpression>(lambdaElement);
            
            return Expression.Invoke(expression, arguments);
        }
    }
}
