using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class MemberMemberBindingInjector : BaseMemberBinding
    {
        public override ExpressionType Type
        {
            get { return ExpressionType.MemberInit; }
        }

        public MemberMemberBindingInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type parentType)
            : base(parent, parentBlock, configurationElement, parentType) { }

        public override void Initialize()
        {
            Type type;
            string name;
            
            BaseInjector<Expression>.TryGetType(this.ConfigurationElement, out type);

            if (!BaseInjector<Expression>.TryGetName(this.ConfigurationElement, out name))
                throw new AttributeNullException(_name);

            this.MemberInfos = type.GetMember(name);

            if (MemberInfos.Length == 0)
                throw new MissingMemberException(this.ParentType.Name, name);

            this.MemberType = type;
        }

        public override MemberBinding Parse()
        {
            IEnumerable<MemberBinding> memberBindings = null;
            
            if (!this.ConfigurationElement.HasElements)
                throw new ArgumentNullException(_expression);

            memberBindings = from expressionElement in this.ConfigurationElement.Elements(_expression)
                             select MemberBindingVisitor.Visit(this, this.ParentBlock, expressionElement, GetMemberType());

            return Expression.MemberBind(this.MemberInfos[0], memberBindings);
        }
    }
}
