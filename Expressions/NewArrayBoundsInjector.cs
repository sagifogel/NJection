using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class NewArrayBoundsInjector : BaseNewArrayInjector
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.NewArrayBounds; }
        }

        protected override string ExpressionElementName
        {
            get { return "bounds"; }
        }

        public NewArrayBoundsInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) :
            base(parent, parentBlock, configurationElement) { }

        protected override NewArrayExpression CreateExpression(Type type, IEnumerable<Expression> expressions)
        {
            if (expressions == null)
                throw new ArgumentNullException("NewArrayBounds");

            return Expression.NewArrayBounds(type, expressions);
        }

        protected override void CheckType(Type expressionType, Type arrayType)
        {
            Type typeOfIntT32 = typeof(int);

            if (!expressionType.Equals(typeOfIntT32))
                throw new ArgumentTypeException(typeOfIntT32, "bound");
        }
    }
}
