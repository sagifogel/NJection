using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public abstract class BaseNewArrayInjector : GenericInjector<NewArrayExpression>
    {
        protected const string _expression = "expression";

        public override abstract ExpressionType Type { get; }
        protected abstract string ExpressionElementName { get; }
        protected abstract void CheckType(Type expressionType, Type arrayType);
        protected abstract NewArrayExpression CreateExpression(Type type, IEnumerable<Expression> expressions);

        public BaseNewArrayInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement) :
            base(parent, parentBlock, configurationElement) { }

        public override NewArrayExpression Parse()
        {
            Type type, initializerType;
            ConstantExpression constant = null;
            List<ConstantExpression> expressions = null;
            XElement expressionElements = this.ConfigurationElement.Element(this.ExpressionElementName);

            TryGetType(this.ConfigurationElement, out type);

            if (expressionElements != null && expressionElements.HasElements)
            {
                expressions = new List<ConstantExpression>(0);

                foreach (XElement expression in expressionElements.Elements(_expression))
                {
                    TryGetType(expression, out initializerType);
                    CheckType(initializerType, type);                    
                    constant = this.Resolve<ConstantExpression>(expression);

                    if (constant == null)
                        throw new ArgumentTypeException(typeof(ConstantExpression), expression.ToString());

                    expressions.Add(constant);
                }
            }

            return CreateExpression(type, expressions);
        }
    }
}

