using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using NJection.Collections;
using NJection.Exceptions;

namespace NJection.Expressions
{
    public class ElementInitInjector : ITreeNode<ElementInit>, IGenericInjector<ElementInit>
    {
        private Type _listType = null;
        private const string _add = "Add";
        private const string _expression = "expression";

        public ExpressionType Type
        {
            get { return ExpressionType.ListInit; }
        }

        object ITreeNodeBase.Value
        {
            get { return this.Value; }
        }

        public ElementInit Value { get; protected set; }

        public string Name { get; protected set; }

        public XElement ConfigurationElement { get; protected set; }

        public IEnumerable<ITreeNode<Expression>> Nodes { get; protected set; }

        public ExpressionRoot Root { get; protected set; }

        public ITreeNodeBase Parent { get; protected set; }

        public ITreeNode<ElementInit> Current { get; protected set; }

        public ITreeNode<Expression> ParentBlock { get; protected set; }

        public ElementInitInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, Type type)
        {
            this.ConfigurationElement = configurationElement;
            this.Current = this;
            this.Parent = parent;
            this.Root = parent.Root;
            this._listType = type;
            this.Value = Parse();
        }

        public ElementInit Parse()
        {
            MethodInfo addMethod = null;
            List<Expression> arguments = null;
            ParameterInfo[] parameters = null;

            addMethod = _listType.GetMethod(_add);

            if (addMethod == null)
                throw new MissingMethodException(_listType.Name, _add);

            parameters = addMethod.GetParameters();

            if (this.ConfigurationElement.HasElements)
            {
                int i = 0;
                arguments = new List<Expression>(0);
                IEnumerable<Expression> expressions = this.ConfigurationElement.Elements(_expression)
                                                                               .Select(expression => 
                                                                                   ExpressionBuilder.Resolve<Expression>(this,  this.ParentBlock, expression));
                foreach (Expression expression in expressions)
                {   
                    if (!expression.Type.Equals(parameters[i].ParameterType))
                        throw new ArgumentTypeException(expression.Type, "expression");

                    arguments.Add(expression);
                    i++;
                }
            }

            if (arguments == null || arguments.Count == 0)
                return Expression.ElementInit(addMethod);

            return Expression.ElementInit(addMethod, arguments);
        }
    }
}
