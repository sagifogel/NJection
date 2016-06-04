using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class MethodCallExpressionInjector : GenericInjector<MethodCallExpression>
    {
        private enum MethodKind
        {
            Static,
            Instance
        }

        private const string _arguments = "arguments";
        private const string _expression = "expression";
        private const string _instance = "instance";
        private const string _methodInfo = "methodInfo";
        private const string _methodName = "methodName";
        private const string _kind = "kind";

        public override ExpressionType Type
        {
            get { return ExpressionType.Call; }
        }

        public MethodCallExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override MethodCallExpression Parse()
        {
            Type type;
            string methodName;
            MethodKind methodKind;
            List<Type> types = null;
            Expression instance = null;
            XElement instanceElement = null;
            List<Expression> expressions = null;
            XAttribute kindAttribute = ConfigurationElement.Attribute(_kind);
            XElement argumentsElement = this.ConfigurationElement.Element(_arguments);

            if (kindAttribute == null || !Enum.TryParse<MethodKind>(kindAttribute.Value, out methodKind))
                throw new AttributeNullException(_kind);

            if (!TryGetAttributeValue(this.ConfigurationElement, out methodName, _methodName))
                throw new AttributeNullException(_methodName);

            if (argumentsElement != null && argumentsElement.HasElements)
            {
                expressions = new List<Expression>();
                types = new List<Type>();

                foreach (XElement expressionElement in argumentsElement.Elements(_expression))
                {
                    Expression expression = this.Resolve<Expression>(expressionElement);
                    types.Add(expression.Type);
                    expressions.Add(expression);
                }
            }

            if (methodKind == MethodKind.Static)
            {
                TryGetType(this.ConfigurationElement, out type);

                if (expressions == null)
                    return Expression.Call(type.GetMethod(methodName));

                return Expression.Call(type.GetMethod(methodName, types.ToArray()), expressions);
            }

            instanceElement = ConfigurationElement.Element(_instance);

            if (instanceElement == null || !instanceElement.HasElements)
                throw new ArgumentNullException(_instance);

            instance = this.Resolve<Expression>(instanceElement.FirstNode as XElement);

            if (expressions == null)
                return Expression.Call(instance, instance.Type.GetMethod(methodName));

            return Expression.Call(instance, instance.Type.GetMethod(methodName, types.ToArray()), expressions);
        }
    }
}