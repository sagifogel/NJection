using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class LoopExpressionInjector : GenericInjector<LoopExpression>
    {
        private const string _expression = "expression";

        public override ExpressionType Type
        {
            get { return ExpressionType.Loop; }
        }

        public LoopExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override LoopExpression Parse()
        {
            GotoExpression gotoExpression;
            ConditionalExpression body = null;
            XElement expressionElement = ConfigurationElement.Element(_expression);
            Dictionary<GotoExpressionKind, LabelTarget> labels = new Dictionary<GotoExpressionKind, LabelTarget>(2);

            if (expressionElement == null)
                throw new ArgumentNullException(_expression);

            body = this.Resolve<ConditionalExpression>(expressionElement);

            if (body == null)
                throw new ArgumentTypeException(typeof(ConditionalExpression), _expression);

            if (TryGetExpression(body.IfTrue, out gotoExpression))
                labels.Add(gotoExpression.Kind, gotoExpression.Target);

            if (TryGetExpression(body.IfFalse, out gotoExpression))
                labels.Add(gotoExpression.Kind, gotoExpression.Target);

            if (labels.Count == 0)
                throw new ArgumentNullException("GotoExpression", "Expression of type Conditional must define a GotoExpresion.");

            if (labels.ContainsKey(GotoExpressionKind.Continue))
                return Expression.Loop(body, labels[GotoExpressionKind.Break], labels[GotoExpressionKind.Continue]);

            return Expression.Loop(body, labels[GotoExpressionKind.Break]);
        }

        private bool TryGetExpression(Expression expression, out GotoExpression gotoExpression)
        {
            gotoExpression = null;

            if (expression is GotoExpression)
            {
                gotoExpression = expression as GotoExpression;
                return true;
            }

            return false;
        }
    }
}
