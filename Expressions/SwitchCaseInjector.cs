using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class SwitchCaseInjector : ITreeNode<SwitchCase>
    {
        private Type _conditionType = null;
        private const string _isDefault = "isDefault";
        private const string _testValues = "testValues";
        private const string _expression = "expression";

        public bool Default { get; private set; }

        public Expression Body { get; private set; }

        object ITreeNodeBase.Value
        {
            get {return this.Value; }
        }

        public SwitchCase Value { get; protected set; }

        public string Name { get; protected set; }

        public XElement ConfigurationElement { get; protected set; }

        public IEnumerable<ITreeNode<Expression>> Nodes { get; protected set; }

        public ExpressionRoot Root { get; protected set; }

        public ITreeNodeBase Parent { get; protected set; }

        public ITreeNode<SwitchCase> Current { get; protected set; }

        public ITreeNode<Expression> ParentBlock { get; protected set; }

        public SwitchCaseInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type switchCondintionType)
        {
            this.ConfigurationElement = configurationElement;
            this.Current = this;
            this.Root = parent.Root;
            this.ParentBlock = parentBlock;
            this.Parent = parent;
            this._conditionType = switchCondintionType;
            this.Value = Parse();
        }

        public ExpressionType Type
        {
            get { return ExpressionType.Switch; }
        }

        public SwitchCase Parse()
        {
            bool isDefault;
            List<Expression> testValues = null;
            XElement expressionElement = this.ConfigurationElement.Element(_expression);
            XElement testValuesElement = this.ConfigurationElement.Element(_testValues);
            XAttribute isDeafultAttribute = this.ConfigurationElement.Attribute(_isDefault);

            if (isDeafultAttribute != null && bool.TryParse(isDeafultAttribute.Value, out isDefault))
                this.Default = isDefault;

            if (expressionElement == null || !expressionElement.HasElements)
                throw new ArgumentNullException(_expression);

            Body = ExpressionBuilder.Resolve<Expression>(this, this.ParentBlock, expressionElement);

            if (testValuesElement != null && testValuesElement.HasElements)
            {
                testValues = new List<Expression>(0);

                foreach (XElement testValueElement in testValuesElement.Elements(_expression))
                {
                    Expression expression = ExpressionBuilder.Resolve<Expression>(this.Parent, this.ParentBlock, testValueElement);

                    if (!expression.Type.Equals(_conditionType))
                        throw new ArgumentTypeException(_conditionType, _expression);

                    testValues.Add(expression);
                }
            }

            if (testValues == null)
                return Expression.SwitchCase(Body, Expression.Constant(string.Empty));

            return Expression.SwitchCase(Body, testValues);
        }       
    }
}
