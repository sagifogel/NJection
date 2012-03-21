using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class ParameterExpressionInjector : GenericInjector<ParameterExpression>
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.Parameter; }
        }

        public ParameterExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock,configurationElement) { }

        public override ParameterExpression Parse()
        {
            Type type;
            string name;

            TryGetType(this.ConfigurationElement, out type);

            if (!TryGetName(this.ConfigurationElement, out name))
                return Expression.Parameter(type);

            return Expression.Parameter(type, name);
        }
    }
}
