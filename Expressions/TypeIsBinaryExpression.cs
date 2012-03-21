using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class TypeIsBinaryExpression : BaseTypeBinaryExpression
    {
        public override System.Linq.Expressions.ExpressionType Type
        {
            get { return ExpressionType.TypeIs; }
        }

        public TypeIsBinaryExpression(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        protected override TypeBinaryExpression CreateTypeBinaryExpression(System.Linq.Expressions.Expression expression, Type type)
        {
            return Expression.TypeIs(expression, type);
        }
    }
}
