using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class NewArrayInitInjector : BaseNewArrayInjector
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.NewArrayInit; }
        }

        protected override string ExpressionElementName
        {
            get { return "initializers"; }
        }

        public NewArrayInitInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) :
            base(parent, parentBlock, configurationElement) { }

        protected override NewArrayExpression CreateExpression(Type type, IEnumerable<Expression> expressions)
        {
            if (expressions == null)
                Expression.NewArrayInit(type);

            return Expression.NewArrayInit(type, expressions);
        }

        protected override void CheckType(Type expressionType, Type arrayType)
        {
            if (!expressionType.Equals(arrayType))
                throw new ArgumentTypeException(expressionType, _expression);
        }
    }
}
