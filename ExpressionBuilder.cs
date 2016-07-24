using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;
using NJection.Expressions;

namespace NJection
{
    public static class ExpressionBuilder
    {
        private static Expression Traverse(XElement configurationElement) {
            return VisitRoot(new ExpressionRoot(), configurationElement.FirstNode as XElement);
        }

        public static Expression<T> Traverse<T>(string filePath) {
            return Traverse<T>(XDocument.Load(filePath));
        }

        public static Expression<T> Traverse<T>(XDocument document) {
            return Traverse(document.FirstNode as XElement) as Expression<T>;
        }

        internal static T Resolve<T>(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) where T : Expression {
            ExpressionType type = BaseInjector<Expression>.ResolveType(configurationElement);
            return Visit(parent, parentBlock, type, configurationElement).Value as T;
        }

        internal static ITreeNode<Expression> Visit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, ExpressionType expressionType, XElement configurationElement) {
            switch (expressionType) {
                case ExpressionType.Block:

                    return VisitBlock(parent, parentBlock, configurationElement);

                case ExpressionType.Parameter:

                    return VisitParameter(parent, parentBlock, configurationElement);

                case ExpressionType.Constant:

                    return VisitConstant(parent, parentBlock, configurationElement);

                case ExpressionType.Label:

                    return VisitLabel(parent, parentBlock, configurationElement);

                case ExpressionType.Loop:

                    return VisitLoop(parent, parentBlock, configurationElement);

                case ExpressionType.Conditional:

                    return VisitConditional(parent, parentBlock, configurationElement);

                case ExpressionType.GreaterThan:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Assign:
                case ExpressionType.MultiplyAssign:

                    return VisitBinary(parent, parentBlock, configurationElement);

                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Increment:
                case ExpressionType.Decrement:
                case ExpressionType.Unbox:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Throw:
                case ExpressionType.OnesComplement:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:

                    return VisitUnary(parent, parentBlock, configurationElement);

                case ExpressionType.Goto:

                    return VisitGoTo(parent, parentBlock, configurationElement);

                case ExpressionType.Lambda:

                    return VisitLambda(parent, parentBlock, configurationElement);

                case ExpressionType.Call:

                    return VisitCall(parent, parentBlock, configurationElement);

                case ExpressionType.Try:

                    return VisitTry(parent, parentBlock, configurationElement);

                case ExpressionType.Switch:

                    return VisitSwitch(parent, parentBlock, configurationElement);

                case ExpressionType.New:

                    return VisitNew(parent, parentBlock, configurationElement);

                case ExpressionType.MemberInit:

                    return VisitMemberInit(parent, parentBlock, configurationElement);

                case ExpressionType.NewArrayInit:

                    return VisitNewArrayInit(parent, parentBlock, configurationElement);

                case ExpressionType.NewArrayBounds:

                    return VisitNewArrayBounds(parent, parentBlock, configurationElement);

                case ExpressionType.ArrayIndex:

                    return VisitArrayIndex(parent, parentBlock, configurationElement);

                case ExpressionType.Index:

                    return VisitArrayAccess(parent, parentBlock, configurationElement);

                case ExpressionType.ListInit:

                    return VisitListInit(parent, parentBlock, configurationElement);

                case ExpressionType.MemberAccess:

                    return VisitMemberAccess(parent, parentBlock, configurationElement);

                case ExpressionType.TypeEqual:

                    return VisitTypeEqual(parent, parentBlock, configurationElement);

                case ExpressionType.TypeIs:

                    return VisitTypeIs(parent, parentBlock, configurationElement);

                case ExpressionType.Invoke:

                    return VisitInvoke(parent, parentBlock, configurationElement);

                default:

                    throw new InvalidExpressionException(expressionType.ToString());
            }
        }

        private static ITreeNode<Expression> VisitInvoke(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new InvocationExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitTypeEqual(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new TypeEqualBinaryExpression(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitTypeIs(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new TypeIsBinaryExpression(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitMemberAccess(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new MemberExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitListInit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new ListInitExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitArrayAccess(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new IndexExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitArrayIndex(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new ArrayIndexInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitNewArrayInit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new NewArrayInitInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitNewArrayBounds(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new NewArrayBoundsInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitMemberInit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new MemberInitExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitNew(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new NewExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitSwitch(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new SwitchExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitTry(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new TryExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitCatchBlock(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new LambdaExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static Expression VisitRoot(ExpressionRoot root, XElement configurationElement) {
            return new LambdaExpressionInjector(root, configurationElement).Parse();
        }

        private static ITreeNode<Expression> VisitBlock(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new BlockExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitParameter(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new ParameterExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitConstant(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new ConstantExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitLabel(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new LabelExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitLoop(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new LoopExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitConditional(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new ConditionalExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitBinary(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new BinaryExpresionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitUnary(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new UnaryExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitGoTo(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new GotoExpressionInjector(parent, parentBlock, configurationElement);
        }

        private static ITreeNode<Expression> VisitLambda(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new LambdaExpressionInjector(parent, null, configurationElement);
        }

        private static ITreeNode<Expression> VisitCall(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) {
            return new MethodCallExpressionInjector(parent, parentBlock, configurationElement);
        }
    }
}


