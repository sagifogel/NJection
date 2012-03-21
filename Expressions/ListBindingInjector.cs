using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Extensions;

namespace NJection.Expressions
{
    public class ListBindingInjector : BaseMemberBinding
    {
        private const string _initializer = "initializer";
        private const string _initializers = "initializers";

        public override ExpressionType Type
        {
            get { return ExpressionType.ListInit; }
        }

        public ListBindingInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type parentType)
            : base(parent, parentBlock, configurationElement, parentType) { }

        public override MemberBinding Parse()
        {
            Type type = GetMemberType();
            IEnumerable<ElementInit> elementInits = null;
            XElement initializers = this.ConfigurationElement.Element(_initializers);
            
            if (initializers != null && initializers.HasElements)
            {   
                elementInits =  from initElement in initializers.Elements(_initializer)
                                select new ElementInitInjector(this, this.ParentBlock, initElement, type).Value;
            }

            return Expression.ListBind(this.MemberInfos[0], elementInits);
        }
    }
}
