using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public abstract class BaseInjector<T> : ITreeNode<T> where T : Expression
    {
        protected const string _ref = "ref";
        protected const string _name = "name";
        protected const string _type = "type";
        protected const string _typeOf = "typeof";

        #region ITreeNode<T> Members

        object ITreeNodeBase.Value
        {
            get { return this.Value; }
        }

        public T Value { get; protected set; }

        public string Name { get; protected set; }

        public XElement ConfigurationElement { get; protected set; }

        public IEnumerable<ITreeNode<Expression>> Nodes { get; protected set; }

        public ExpressionRoot Root { get; protected set; }

        public ITreeNodeBase Parent { get; protected set; }

        public ITreeNode<T> Current { get; protected set; }

        public ITreeNode<Expression> ParentBlock { get; protected set; }

        public BaseInjector()
        {
            Nodes = new List<ITreeNode<T>>(0);
        }

        #endregion

        protected ITreeNode<TExpression> ResolveTreeNode<TExpression>(XElement configurationElement) where TExpression : Expression
        {
            ITreeNode<Expression> block = this is IBlockInjector ? this : this.ParentBlock;
            ITreeNode<TExpression> target = this.Find<TExpression>(configurationElement);

            if (target != null)
                return target;

            return ExpressionBuilder.Visit(this, block, ResolveType(configurationElement), configurationElement) as ITreeNode<TExpression>;
        }

        public TExpression Resolve<TExpression>(XElement configurationElement) where TExpression : Expression
        {
            return this.ResolveTreeNode<TExpression>(configurationElement).Value as TExpression;
        }

        protected ITreeNode<TExpression> Find<TExpression>(XElement configurationElement) where TExpression : Expression
        {
            string name;
            ITreeNode<TExpression> target = null;

            if (TryGetReferenceName(configurationElement, out name))
                target = this.Find<TExpression>(name);

            return target;
        }

        protected ITreeNode<TExpression> Find<TExpression>(string name) where TExpression : Expression
        {
            ITreeNodeBase node = this;
            ITreeNode<TExpression> element = null;
            IDictionary<string, object> definitions = null;

            do
            {
                if (node is IBlockInjector)
                {
                    definitions = (node as IBlockInjector).Definitions;

                    if (definitions.ContainsKey(name))
                        return definitions[name] as ITreeNode<TExpression>;
                }

                node = node.Parent;
            }
            while (node != null);

            return element;
        }

        public static bool TryGetType(string typeName, out Type type)
        {
            type = System.Type.GetType(typeName);

            return type != null;
        }

        public static bool TryGetType(XElement configurationElement, out Type type)
        {
            string typeOf;

            if (!TryGetAttributeValue(configurationElement, out typeOf, _typeOf))
                throw new AttributeNullException(_typeOf);

            return TryGetType(typeOf, out type);
        }

        public static ExpressionType ResolveType(XElement configurationElement)
        {
            return ResolveType<ExpressionType>(configurationElement);
        }

        public static TStruct ResolveType<TStruct>(XElement configurationElement) where TStruct : struct
        {   
            string type;

            if (!TryGetAttributeValue(configurationElement, out type, _type))
                throw new AttributeNullException(_type);

            return (TStruct)Enum.Parse(typeof(TStruct), type);
        }

        public static bool TryGetReferenceName(XElement configurationElement, out string name)
        {
            return TryGetAttributeValue(configurationElement, out name, _ref);
        }

        public bool TryAssignName(XElement configurationElement)
        {   
            string name;

            if (TryGetAttributeValue(configurationElement, out name, _name))
            {
                this.Name = name;
                return true;
            }
            
            return false;
        }

        public static bool TryGetName(XElement configurationElement, out string name)
        {
            return TryGetAttributeValue(configurationElement, out name, _name);
        }    

        public static bool TryGetAttributeValue(XElement configurationElement, out string value, string attributeName)
        {
            XAttribute attribute = configurationElement.Attribute(attributeName);

            value = string.Empty;

            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Value))
            {
               value = attribute.Value;
               return true;
            }

            return false;
        }    
    }
}
