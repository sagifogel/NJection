using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using NJection.Collections;

namespace NJection.Expressions
{
    public class DummyNode<T> : ITreeNode<T>, IGenericInjector<T> where T :class
    {
        #region ITreeNode<Expression> Members

        public DummyNode() { }

        public DummyNode(T value)
        {
            this.Value = value;
        }

        public string Name { get; set; }

        object ITreeNodeBase.Value
        {
            get { return this.Value; }
        }

        public T Value { get; set; }

        public XElement ConfigurationElement { get; set; }

        public IEnumerable<ITreeNode<Expression>> Nodes { get; set; }

        public ExpressionRoot Root { get; set; }

        public ITreeNodeBase Parent { get; protected set; }

        public ITreeNode<T> Current { get; set; }

        public ITreeNode<Expression> ParentBlock { get; set; }        

        #endregion

        public virtual T Parse()
        {
            throw new NotImplementedException();
        }

        public virtual ExpressionType Type 
        {
            get { throw new NotImplementedException(); } 
        }
    }
}
