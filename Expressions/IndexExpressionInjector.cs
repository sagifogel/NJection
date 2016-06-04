using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class IndexExpressionInjector : BaseArrayInjector<IndexExpression>
    {
        public IndexExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.Index; }
        }

        protected override IndexExpression CreateExpression(Expression arrayExpression, IEnumerable<Expression> indexes)
        {
            return Expression.ArrayAccess(arrayExpression, indexes);
        }
    }
}
