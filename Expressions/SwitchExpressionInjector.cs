using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Xml.XPath;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class SwitchExpressionInjector : GenericInjector<SwitchExpression>
    {
        private const string _switchValue = "switchValue";
        private const string _switchCases = "switchCases";

        public SwitchExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.Switch; }
        }

        public override SwitchExpression Parse()
        {
            Type conditionType = null;
            Expression switchValueExpression = null;
            Expression defaultExpressionBody = null;
            List<SwitchCase> switchCases = new List<SwitchCase>(0);
            XElement switchValueElement = this.ConfigurationElement.Element(_switchValue);
            XElement switchCasesElement = this.ConfigurationElement.Element(_switchCases);

            if (switchValueElement == null || !switchValueElement.HasElements)
                throw new ArgumentNullException(_switchValue);

            switchValueExpression = this.Resolve<Expression>(switchValueElement.FirstNode as XElement);

            if (TryGetType(this.ConfigurationElement, out conditionType))
            {
                if (!conditionType.Equals(switchValueExpression.Type))
                    throw new ArgumentTypeException(switchValueExpression.Type, "expression");
            }
            else
                conditionType = switchValueExpression.Type;

            if (switchCasesElement == null || !switchCasesElement.HasElements)
                throw new ArgumentNullException(_switchCases);

            foreach (XElement switchCaseElement in switchCasesElement.XPathSelectElements("expression[@type=\"SwitchCase\"]"))
            {
                SwitchCaseInjector switchCaseInjector = new SwitchCaseInjector(this, this.ParentBlock, switchCaseElement, conditionType);

                if (switchCaseInjector.Default)
                    defaultExpressionBody = switchCaseInjector.Body;
                else
                    switchCases.Add(switchCaseInjector.Value);
            }

            if (defaultExpressionBody == null)
                return Expression.Switch(switchValueExpression, switchCases.ToArray());

            return Expression.Switch(switchValueExpression, defaultExpressionBody, switchCases.ToArray());
        }
    }
}
