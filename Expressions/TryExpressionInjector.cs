using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Xml.XPath;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class TryExpressionInjector : BaseBlockInjector<TryExpression>
    {
        private const string _finally = "finally"; 
        private const string _catchBlocks = "catchBlocks";

        public override ExpressionType Type
        {
            get { return ExpressionType.Try; }
        }

        public TryExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) :
            base(parent, parentBlock, configurationElement) { }

        public override TryExpression Parse()
        {
            Type type;
            string name;
            Expression expression = null;
            Expression finallyExpression = null;
            List<CatchBlock> catchBlocks = new List<CatchBlock>(0);
            XElement finallyElement = ConfigurationElement.Element(_finally);
            XElement expressionElement = ConfigurationElement.Element(_expression);
            XElement catchBlockElement = ConfigurationElement.Element(_catchBlocks);

            if (expressionElement == null || !expressionElement.HasElements)
                throw new ArgumentNullException(_expression);

            expression = this.Resolve<Expression>(expressionElement);

            if (catchBlockElement != null && catchBlockElement.HasElements)
            {
                foreach (XElement catchElement in catchBlockElement.XPathSelectElements("expression[@type=\"Catch\"]"))
                {
                    CatchBlock catchBlock = null;
                    CatchBlockInjector catchBlockInjector = null;
                    ParameterExpression parameterExpression = null;

                    TryGetType(catchElement, out type);

                    if (TryGetName(catchElement, out name))
                    {
                        parameterExpression = Expression.Parameter(type, name);
                        this.Definitions.Add(name, new DummyNode<ParameterExpression>(parameterExpression));
                    }
                    else
                        parameterExpression = Expression.Parameter(type);

                    catchBlockInjector = new CatchBlockInjector(this, this.ParentBlock, catchElement, parameterExpression);
                    catchBlock = catchBlockInjector.Value;
                    catchBlocks.Add(catchBlock);

                    if (!catchBlock.Body.Type.Equals(expression.Type))
                        throw new ArgumentTypeException(catchBlock.Body.Type, _expression,
                                                        "The return type of the try block must match the return type of any associated catch statements.");
                }
            }

            if (finallyElement != null && finallyElement.HasElements)
            {
                finallyExpression = Resolve<Expression>(finallyElement.FirstNode as XElement);

                if (catchBlocks.Count > 0)
                    return Expression.TryCatchFinally(expression, finallyExpression, catchBlocks.ToArray());
                else
                    return Expression.TryFinally(expression, finallyExpression);
            }

            return Expression.TryCatch(expression, catchBlocks.ToArray());
        }
    }
}
