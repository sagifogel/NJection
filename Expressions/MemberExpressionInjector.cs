using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class MemberExpressionInjector : GenericInjector<MemberExpression>
    {   
        private const string _expression = "expression";
        private const string _memberName = "memberName";

        public MemberExpressionInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public override ExpressionType Type
        {
            get { return ExpressionType.MemberAccess; }
        }

        public override MemberExpression Parse()
        {
            string memberName;
            Expression expression = null;
            XElement expressionElement = this.ConfigurationElement.Element(_expression);

            if (!TryGetAttributeValue(this.ConfigurationElement, out memberName, _memberName))
                throw new AttributeNullException(_memberName);

            expression = this.Resolve<Expression>(expressionElement);

            return Expression.PropertyOrField(expression, memberName);
        }
    }
}
