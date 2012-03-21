using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class LabelExpressionInjector : GenericInjector<LabelExpression>
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.Label; }
        }

        public LabelExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override LabelExpression Parse()
        {
            Type type;
            string name;
            LabelTarget label = null;
            LabelExpression labelExpression = null;

            if (!TryGetName(this.ConfigurationElement, out name))
                throw new ArgumentNullException(_name);

            if (TryGetType(this.ConfigurationElement, out type))
            {
                label = Expression.Label(type, name);
                object value = TypeDescriptor.CreateInstance(null, type, null, null);
                labelExpression = Expression.Label(label, Expression.Constant(value));
            }
            else
            {
                label = Expression.Label(name);
                labelExpression = Expression.Label(label);
            }

            return labelExpression;
        }
    }
}
