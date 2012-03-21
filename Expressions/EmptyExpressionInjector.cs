using System.Linq.Expressions;
using NJection.Collections;
namespace NJection.Expressions
{
    public class EmptyExpressionInjector : GenericInjector<DefaultExpression>
    {
        public EmptyExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock)
            : base(parent, parentBlock, null) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.Default; }
        }

        public override DefaultExpression Parse()
        {
            return Expression.Empty();
        }
    }
}
