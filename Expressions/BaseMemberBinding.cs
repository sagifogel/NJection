using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public abstract class BaseMemberBinding : DummyNode<MemberBinding>
    {
        protected const string _ref = "ref";
        protected const string _type = "type";
        protected const string _name = "name";
        protected const string _typeOf = "typeof";
        protected const string _expression = "expression";

        protected Type MemberType { get; set; }
        protected Type ParentType { get; set; }
        protected MemberInfo[] MemberInfos { get; set; }
        protected XAttribute NameAttribute { get; set; }

        public abstract override MemberBinding Parse();
        public abstract override ExpressionType Type { get; }

        public BaseMemberBinding(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type parentType)
        {
            this.ConfigurationElement = configurationElement;
            this.Current = this;
            this.Root = parent.Root;
            this.ParentType = parentType;
            this.ParentBlock = parentBlock;
            this.Parent = parent;
            Initialize();
            this.Value = Parse();
        }

        public virtual void Initialize()
        {
            Type type;
            string name;

            BaseInjector<Expression>.TryGetType(this.ConfigurationElement, out type);

            if (!BaseInjector<Expression>.TryGetName(this.ConfigurationElement, out name))
                throw new AttributeNullException(_name);

            this.Name = name;
            this.MemberInfos = ParentType.GetMember(name);

            if (MemberInfos.Length == 0)
                throw new MissingMemberException(this.ParentType.Name, name);

            this.MemberType = type;
        }

        internal static MemberBindingType ResolveMemberBindType(XElement configurationElement)
        {
            MemberBindingType memberBindingType;
            
            TryResolveMemberBindType(configurationElement, out memberBindingType);
            return memberBindingType;
        }

        protected static bool TryResolveMemberBindType(XElement configurationElement, out MemberBindingType memberBindingType)
        {
            string memberBindingName;

            BaseInjector<Expression>.TryGetAttributeValue(configurationElement, out memberBindingName, _type);
            return Enum.TryParse<MemberBindingType>(memberBindingName, out memberBindingType);
        }

        protected Type GetMemberType()
        {
            MemberInfo memberInfo = this.MemberInfos[0];

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:

                    return (memberInfo as FieldInfo).FieldType;

                case MemberTypes.Property:
                    return (memberInfo as PropertyInfo).PropertyType;

                default:

                    throw new MemberAccessException(string.Format("Member type should be Field or Property. {0} is of type {1} which is not supported.", this.Name, memberInfo.MemberType.ToString()));
            }
        }
    }
}
