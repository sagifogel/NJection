using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public abstract class BaseBlockInjector<T> : GenericInjector<T>, IBlockInjector where T : Expression
    {
        protected const string _expression = "expression";
        protected const string _definitions = "definitions";        
        protected IDictionary<string, object> DefinitionsDictionary = new Dictionary<string, object>(0);

        #region IExpressionBlock Members

        public IDictionary<string, object> Definitions
        {
            get { return DefinitionsDictionary; }
            protected set { DefinitionsDictionary = value; }
        }

        public BaseBlockInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement)
            : base(parent, parentBlock, configurationElement) { }

        public BaseBlockInjector() { }

        public Dictionary<string, object> ResolveDefinitions(IEnumerable<XElement> definitions)
        {
            object expression = null;
            XAttribute nameAttribute = null;
            Dictionary<string, object> resolvedDefinitions = new Dictionary<string, object>();
            
            foreach (XElement definition in definitions)
            {   
                string name;

                if (!TryGetName(definition, out name))
                    throw new AttributeNullException(_name);

                expression = ExpressionBuilder.Visit(this, this, ResolveType(definition), definition);

                if (resolvedDefinitions.ContainsKey(name))
                    throw new ArgumentException(string.Format("An item with the same name has already been defined.{0}Parameter name: {1}.", Environment.NewLine, nameAttribute.Value));

                resolvedDefinitions.Add(name, expression);
            }

            return resolvedDefinitions;
        }

        #endregion
    }
}
