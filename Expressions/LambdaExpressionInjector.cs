using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class LambdaExpressionInjector : BaseBlockInjector<LambdaExpression>, IBlockInjector
    {
        private const string _arguments = "arguments";
        List<ITreeNode<Expression>> _argumentsList = new List<ITreeNode<Expression>>(0);

        public override ExpressionType Type
        {
            get { return ExpressionType.Lambda; }
        }

        internal Type ReturnType { get; private set; }

        public LambdaExpressionInjector(ExpressionRoot root, XElement configurationElement)
        {
            this.Root = root;
            root.First = this;
            this.Current = this;
            this.ConfigurationElement = configurationElement;
        }

        public LambdaExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override LambdaExpression Parse()
        {
            Type type;
            Expression body = null;
            XElement argumentsElement = ConfigurationElement.Element(_arguments);
            XElement expressionElement = ConfigurationElement.Element(_expression);
            XElement definitionsElement = ConfigurationElement.Element(_definitions);

            TryGetType(this.ConfigurationElement, out type);

            if (!type.IsSubclassOf(typeof(Delegate)))
                throw new ArgumentTypeException(typeof(LambdaExpression), _expression);

            if (expressionElement == null || !expressionElement.HasElements)
                throw new ArgumentNullException(_expression);

            if (definitionsElement != null)
                Definitions = this.ResolveDefinitions(definitionsElement.Elements(_expression));

            if (argumentsElement != null)
                this.ResolveArguments(argumentsElement.Elements(_expression), type);

            body = this.Resolve<Expression>(expressionElement);

            if (_argumentsList == null || _argumentsList.Count == 0)
                return Expression.Lambda(type, body);

            return Expression.Lambda(type, body, _argumentsList.Select(argument => argument.Value as ParameterExpression));
        }

        private void ResolveArguments(IEnumerable<XElement> variables, Type type, XElement returnElement = null)
        {
            MethodInfo methodInfo = type.GetMethod("Invoke");

            if (methodInfo.ReturnType == typeof(void))
                ResolveActionArguments(variables, type);
            else
                ResolveFuncArguments(variables, type, methodInfo.ReturnType);
        }

        private void ResolveFuncArguments(IEnumerable<XElement> variables, Type methodType, Type returnType)
        {
            int variablesCount = variables.Count();
            Type[] types = methodType.GetGenericArguments();

            if (types.Length - 1 != variablesCount)
                throw new ArgumentException("Arguments do not correspond to funcion type or length.");

            this.ReturnType = returnType;

            for (int i = 0; i < variablesCount; i++)
                ResolveArgument(variables.ElementAt(i), types[i]);
        }

        private void ResolveActionArguments(IEnumerable<XElement> variables, Type methodType)
        {
            int variablesCount = variables.Count();
            Type[] types = methodType.GetGenericArguments();

            if (types.Length - variablesCount != 0)
                throw new ArgumentException("Arguments do not correspond to funcion type or length.");

            for (int i = 0; i < variablesCount; i++)
                this.ResolveArgument(variables.ElementAt(i), types[i]);
        }

        private void ResolveArgument(XElement configurationElement, Type type)
        {
            string name;
            Type parameterType;
            ITreeNode<Expression> expression = null;
            XAttribute refAttribute = configurationElement.Attribute(_ref);
            XAttribute typeAttribute = configurationElement.Attribute(_typeOf);

            if (TryGetReferenceName(configurationElement, out name))
            {
                expression = this.Find<Expression>(configurationElement);

                if (!(expression.Value is Expression))
                    throw new ArgumentTypeException(typeof(Expression), _expression);
                else if ((expression.Value as Expression).Type != type)
                    throw new ArgumentTypeException(type, _expression);
            }
            else
            {
                TryGetType(configurationElement, out parameterType);

                if (!parameterType.Equals(type))
                    throw new ArgumentTypeException(type, _expression);

                if (!TryGetName(configurationElement, out name))
                    throw new AttributeNullException(_name);

                expression = ExpressionBuilder.Visit(this, this, ResolveType(configurationElement), configurationElement);
            }

            _argumentsList.Add(expression);
        }
    }
}
