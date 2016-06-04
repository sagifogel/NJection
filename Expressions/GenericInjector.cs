using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public abstract class GenericInjector<T> : BaseInjector<T>, IGenericInjector<T>
        where T : Expression
    {
        public GenericInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock, XElement configurationElement, object[] args = null)
        {
            this.ConfigurationElement = configurationElement;
            this.Current = this;
            this.Parent = parent;
            this.ParentBlock = parentBlock;

            if (parent != null)
                this.Root = parent.Root;

            Create();
        }

        public GenericInjector() { }

        public GenericInjector(ITreeNodeBase parent, ITreeNode<Expression> parentBlock,  XElement configurationElement, ExpressionRoot root)
            : this(parent, parentBlock, configurationElement)
        {
            this.Root = root;
        }

        #region IGenericInjector<T> Members

        public virtual void Create()
        {
            this.Value = Parse();
            
            if (TryAssignName(this.ConfigurationElement) && this.ParentBlock != null)
            {
                IBlockInjector block = null; 
                ITreeNode<Expression> node = this.ParentBlock;
                
                do
                {       
                    block = node as IBlockInjector;

                    if (block.Definitions.ContainsKey(this.Name))
                        throw new ArgumentException(string.Format("An item with the same name has already been defined.{0}Parameter name: {1}.", Environment.NewLine, this.Name));

                    node = node.ParentBlock;
                }
                while (node != null);

                block.Definitions.Add(this.Name, this);
            }
        }

        public abstract T Parse();
        public abstract ExpressionType Type { get; }

       #endregion
    }
}
