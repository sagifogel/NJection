using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class ArrayIndexInjector : BaseArrayInjector<MethodCallExpression>
    {
        public ArrayIndexInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.ArrayIndex; }
        }

        protected override MethodCallExpression CreateExpression(Expression arrayExpression, IEnumerable<Expression> indexes)
        {   
            return Expression.ArrayIndex(arrayExpression, indexes);
        }
    }
}
