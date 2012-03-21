using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class BinaryExpresionInjector : GenericInjector<BinaryExpression>
    {
        private const string _left = "left";
        private const string _right = "right";

        public override ExpressionType Type
        {
            get { return Value.NodeType; }
        }

        public BinaryExpresionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override BinaryExpression Parse()
        {
            XElement leftElement = ConfigurationElement.Element(_left);
            XElement rightElement = ConfigurationElement.Element(_right);

            if (leftElement == null || !leftElement.HasElements)
                throw new ArgumentNullException(_left);

            if (rightElement == null || !rightElement.HasElements)
                throw new ArgumentNullException(_right);

            Expression left = this.Resolve<Expression>(leftElement.FirstNode as XElement);
            Expression right = this.Resolve<Expression>(rightElement.FirstNode as XElement);
            
            this.Value = VisitBinary(ResolveType(ConfigurationElement), left, right);
            
            return this.Value;
        }

        private BinaryExpression VisitBinary(ExpressionType type, Expression left, Expression right)
        {
            switch (type)
            {
                case ExpressionType.Add:

                    return Expression.Add(left, right);

                case ExpressionType.AddChecked:

                    return Expression.AddChecked(left, right);

                case ExpressionType.AddAssign:

                    return Expression.AddAssign(left, right);

                case ExpressionType.AddAssignChecked:

                    return Expression.AddAssignChecked(left, right);

                case ExpressionType.Subtract:

                    return Expression.Subtract(left, right);

                case ExpressionType.SubtractChecked:

                    return Expression.SubtractChecked(left, right);

                case ExpressionType.SubtractAssign:

                    return Expression.SubtractAssign(left, right);

                case ExpressionType.SubtractAssignChecked:

                    return Expression.SubtractAssignChecked(left, right);

                case ExpressionType.Multiply:

                    return Expression.Multiply(left, right);

                case ExpressionType.MultiplyAssign:

                    return Expression.MultiplyAssign(left, right);

                case ExpressionType.MultiplyChecked:

                    return Expression.MultiplyChecked(left, right);

                case ExpressionType.MultiplyAssignChecked:

                    return Expression.MultiplyAssignChecked(left, right);

                case ExpressionType.Divide:

                    return Expression.Divide(left, right);

                case ExpressionType.DivideAssign:

                    return Expression.DivideAssign(left, right);

                case ExpressionType.Modulo:

                    return Expression.Modulo(left, right);

                case ExpressionType.ModuloAssign:

                    return Expression.ModuloAssign(left, right);

                case ExpressionType.And:

                    return Expression.And(left, right);

                case ExpressionType.AndAlso:

                    return Expression.AndAlso(left, right);

                case ExpressionType.Or:

                    return Expression.Or(left, right);

                case ExpressionType.OrElse:

                    return Expression.OrElse(left, right);

                case ExpressionType.OrAssign:

                    return Expression.OrAssign(left, right);

                case ExpressionType.LessThan:

                    return Expression.LessThan(left, right);

                case ExpressionType.LessThanOrEqual:

                    return Expression.LessThanOrEqual(left, right);

                case ExpressionType.GreaterThan:

                    return Expression.GreaterThan(left, right);

                case ExpressionType.GreaterThanOrEqual:

                    return Expression.GreaterThanOrEqual(left, right);

                case ExpressionType.Equal:

                    return Expression.Equal(left, right);

                case ExpressionType.NotEqual:

                    return Expression.NotEqual(left, right);

                case ExpressionType.Coalesce:

                    return Expression.Coalesce(left, right);

                case ExpressionType.RightShift:

                    return Expression.RightShift(left, right);

                case ExpressionType.RightShiftAssign:

                    return Expression.RightShiftAssign(left, right);

                case ExpressionType.LeftShift:

                    return Expression.LeftShift(left, right);

                case ExpressionType.LeftShiftAssign:

                    return Expression.LeftShiftAssign(left, right);

                case ExpressionType.ExclusiveOr:

                    return Expression.ExclusiveOr(left, right);

                case ExpressionType.ExclusiveOrAssign:

                    return Expression.ExclusiveOrAssign(left, right);

                case ExpressionType.Assign:

                    return Expression.Assign(left, right);

                case ExpressionType.AndAssign:

                    return Expression.AndAssign(left, right);

                case ExpressionType.PowerAssign:

                    return Expression.PowerAssign(left, right);

                case ExpressionType.Power:

                    return Expression.Power(left, right);

                default:

                    throw new InvalidExpressionException(type.ToString());
            }
        }
    }
}
