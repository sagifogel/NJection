using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class UnaryExpressionInjector : GenericInjector<UnaryExpression>
    {
        private const string _expression = "expression";

        public override ExpressionType Type
        {
            get { return Value.NodeType; }
        }

        public UnaryExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override UnaryExpression Parse()
        {
            Type type;            
            Expression expression = null;
            XAttribute typeElement = ConfigurationElement.Attribute(_type);
            XElement expressionElement = ConfigurationElement.Element(_expression);

            if (expressionElement == null)
                throw new ArgumentNullException(_expression);

            expression = this.Resolve<Expression>(expressionElement);

            if (typeElement != null && TryGetType(typeElement.Value, out type))
                Value = VisitUnary(ResolveType(ConfigurationElement), expression, type);
            else
                Value = VisitUnary(ResolveType(ConfigurationElement), expression);

            return Value;
        }

        private UnaryExpression VisitUnary(ExpressionType expressionType, Expression expression, Type type = null)
        {
            switch (expressionType)
            {
                case ExpressionType.Negate:

                    return Expression.Negate(expression);

                case ExpressionType.NegateChecked:

                    return Expression.NegateChecked(expression);

                case ExpressionType.Not:

                    return Expression.Not(expression);

                case ExpressionType.Convert:

                    return Expression.Convert(expression, type);

                case ExpressionType.ConvertChecked:

                    return Expression.ConvertChecked(expression, type);

                case ExpressionType.ArrayLength:

                    return Expression.ArrayLength(expression);

                case ExpressionType.Quote:

                    return Expression.Quote(expression);

                case ExpressionType.TypeAs:

                    return Expression.TypeAs(expression, type);

                case ExpressionType.PreDecrementAssign:

                    return Expression.PreDecrementAssign(expression);

                case ExpressionType.PostDecrementAssign:

                    return Expression.PostDecrementAssign(expression);

                case ExpressionType.PreIncrementAssign:

                    return Expression.PreIncrementAssign(expression);

                case ExpressionType.PostIncrementAssign:

                    return Expression.PostIncrementAssign(expression);

                case ExpressionType.Increment:

                    return Expression.Increment(expression);

                case ExpressionType.Decrement:

                    return Expression.Decrement(expression);

                case ExpressionType.Unbox:

                    return Expression.Unbox(expression, type);

                case ExpressionType.UnaryPlus:

                    return Expression.UnaryPlus(expression);

                case ExpressionType.Throw:

                    if (expression == null && type == null)
                        return Expression.Rethrow();
                    else if (expression != null && type != null)
                        return Expression.Throw(expression, type); 
                    else if (expression != null && type == null)
                        return Expression.Throw(expression);
                    
                    return Expression.Rethrow(type);

                case ExpressionType.OnesComplement:

                    return Expression.OnesComplement(expression);

                case ExpressionType.IsTrue:

                    return Expression.IsTrue(expression);

                case ExpressionType.IsFalse:

                    return Expression.IsFalse(expression);

                default:

                    throw new InvalidExpressionException(expressionType.ToString());
            }
        }
    }
}
