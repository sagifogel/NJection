using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;


namespace NJection.Expressions
{
    public class CatchBlockInjector : ITreeNode<CatchBlock>, IGenericInjector<CatchBlock>
    {
        private const string _expression = "expression";
        private ParameterExpression _parameterExpression = null;

        object ITreeNodeBase.Value
        {
            get { return this.Value; }
        }

        public CatchBlock Value { get; protected set; }

        public string Name { get; protected set; }

        public XElement ConfigurationElement { get; protected set; }

        public IEnumerable<ITreeNode<Expression>> Nodes { get; protected set; }

        public ExpressionRoot Root { get; protected set; }

        public ITreeNodeBase Parent { get; protected set; }

        public ITreeNode<CatchBlock> Current { get; protected set; }

        public ITreeNode<Expression> ParentBlock { get; protected set; }

        public ExpressionType Type
        {
            get { return ExpressionType.Try; }
        }

        public CatchBlockInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, ParameterExpression parameterExprssion)           
        {
            this.ConfigurationElement = configurationElement;
            this.Current = this;
            this.Root = parent.Root;
            this.Parent = parent;
            this._parameterExpression = parameterExprssion;
            this.Value = Parse();
        }

        public CatchBlock Parse()
        {
            Expression expression = null;
            XElement expressionElement = ConfigurationElement.Element(_expression);

            if (expressionElement == null)
                throw new ArgumentNullException(_expression);

            expression = ExpressionBuilder.Resolve<Expression>(this.Parent, this.ParentBlock, expressionElement);

            return Expression.Catch(_parameterExpression, expression);
        }
    }
}
