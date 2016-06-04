using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class MemberAssignmentInjector : BaseMemberBinding
    {
        public MemberAssignmentInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type parentType)
            : base(parent, parentBlock, configurationElement, parentType) { }
    
        public override ExpressionType Type
        {
            get { return ExpressionType.MemberInit; }
        }

        public override MemberBinding Parse()
        {
            ConstantExpression expression = null; 
            PropertyInfo propertyInfo = ParentType.GetProperty(this.Name);

            expression = ExpressionBuilder.Resolve<ConstantExpression>(this, this.ParentBlock, this.ConfigurationElement);

            if (expression == null)
                throw new ArgumentTypeException(typeof(ConstantExpression), _expression);

            if (!propertyInfo.PropertyType.Equals(expression.Type) || !this.MemberType.Equals(expression.Type))
                throw new ArgumentTypeException(expression.Type, this.Name);

            return Expression.Bind(this.MemberInfos[0], expression);
        }
    }
}
