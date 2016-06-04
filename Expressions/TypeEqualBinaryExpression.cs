using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class TypeEqualBinaryExpression : BaseTypeBinaryExpression
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.TypeIs; }
        }

        public TypeEqualBinaryExpression(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        protected override TypeBinaryExpression CreateTypeBinaryExpression(Expression expression, Type type)
        {
            return Expression.TypeEqual(expression, type);
        }
    }
}