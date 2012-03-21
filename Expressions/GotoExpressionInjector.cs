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
    public class GotoExpressionInjector : GenericInjector<GotoExpression>
    {
        private const string _kind = "kind";
        private const string _label = "label";
        private const string _expression = "expression";

        public override ExpressionType Type
        {
            get { return ExpressionType.Goto; }
        }

        public GotoExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override GotoExpression Parse()
        {
            MethodInfo methodInfo = null;
            LabelTarget labelTarget = null;
            GotoExpressionKind kind = GotoExpressionKind.Goto;
            XAttribute kindAttribute = ConfigurationElement.Attribute(_kind);
            XElement labelElement = this.ConfigurationElement.Element(_label);
            XElement expressionElement = ConfigurationElement.Element(_expression);

            if (kindAttribute == null || !Enum.TryParse<GotoExpressionKind>(kindAttribute.Value, out kind))
                throw new AttributeNullException(_kind);

            if (labelElement == null || !labelElement.HasElements)
                throw new ArgumentNullException(_label);

            labelTarget = this.Resolve<LabelExpression>(labelElement.FirstNode as XElement).Target;
            List<object> arguments = new List<object>() { labelTarget };

            if (expressionElement != null)
                arguments.Add(this.Resolve<Expression>(expressionElement));

            methodInfo = typeof(Expression).GetMethod(kind.ToString(), BindingFlags.Static | BindingFlags.Public, null, arguments.Select(arg => arg.GetType()).ToArray(), null);
            return methodInfo.Invoke(null, arguments.ToArray()) as GotoExpression;
        }
    }
}

