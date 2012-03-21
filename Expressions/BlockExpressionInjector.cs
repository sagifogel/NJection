using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Extensions;

namespace NJection.Expressions
{
    public class BlockExpressionInjector : BaseBlockInjector<BlockExpression>
    {
        private const string _result = "result";
        private const string _arguments = "arguments";
        private const string _expressions = "expressions";

        public override ExpressionType Type
        {
            get { return ExpressionType.Block; }
        }

        public BlockExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override BlockExpression Parse()
        {
            IEnumerable<Expression> expressions = null;
            IEnumerable<ParameterExpression> arguments = null;
            XElement result = ConfigurationElement.Element(_result);
            XElement definitionsElement = ConfigurationElement.Element(_definitions);
            XElement argumentElement = ConfigurationElement.Element(_arguments);
            XElement expressionsElement = ConfigurationElement.Element(_expressions);
            
            if (definitionsElement != null)
                Definitions = this.ResolveDefinitions(definitionsElement.Elements(_expression));

            if (argumentElement != null)
            {
                arguments = from argument in argumentElement.Elements(_expression)
                            select this.Resolve<ParameterExpression>(argument);
            }

            if (expressionsElement == null)
                throw new ArgumentNullException(_expressions);

            expressions = from exp in expressionsElement.Elements(_expression)
                          select this.Resolve<Expression>(exp);

            if (!arguments.IsNullOrEmpty())
                return Expression.Block(arguments, expressions);

            return Expression.Block(expressions);
        }
    }
}
