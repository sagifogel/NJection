using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public static class MemberBindingVisitor
    {
        public static MemberBinding Visit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type parentType)
        {
            MemberBindingType memberBindingType = BaseMemberBinding.ResolveMemberBindType(configurationElement);
            return Visit(parent, parentBlock, memberBindingType, configurationElement, parentType);
        }

        public static MemberBinding Visit(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, MemberBindingType memberBindingType, XElement configurationElement, Type parentType)
        {
            BaseMemberBinding memberBinding = null;

            switch (memberBindingType)
            {
                case MemberBindingType.ListBinding:

                    memberBinding = new ListBindingInjector(parent, parentBlock, configurationElement, parentType);
                    break;

                case MemberBindingType.MemberBinding:

                    memberBinding = new MemberMemberBindingInjector(parent, parentBlock, configurationElement, parentType);
                    break;

                case MemberBindingType.Assignment:
                default:
                    memberBinding = new MemberAssignmentInjector(parent, parentBlock, configurationElement, parentType);
                    break;
            }

            return memberBinding.Value;
        }
    }
}
