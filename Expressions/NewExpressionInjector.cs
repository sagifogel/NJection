using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Extensions;

namespace NJection.Expressions
{
    public class NewExpressionInjector : GenericInjector<NewExpression>
    {
        private const string _arguments = "arguments";
        private const string _expression = "expression";

        public NewExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.New; }
        }

        public override NewExpression Parse()
        {
            Type type = null;
            List<Type> types = new List<Type>(0);
            IEnumerable<Expression> arguments = null;
            XElement argumentsElement = this.ConfigurationElement.Element(_arguments);
            Func<XElement, Expression> resolve = (XElement argumentElement) =>
            {
                Expression expression = this.Resolve<Expression>(argumentElement);
                types.Add(expression.Type);

                return expression;
            };

            TryGetType(this.ConfigurationElement, out type);

            if (argumentsElement != null && argumentsElement.HasElements)
            {
                arguments = (from argument in argumentsElement.Elements(_expression)
                            select resolve(argument)).ToList();
            }

            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, types.ToArray(), null);
            
            if (arguments.IsNullOrEmpty())
                return Expression.New(constructorInfo);

            return Expression.New(constructorInfo, arguments);
        }
    }
}
