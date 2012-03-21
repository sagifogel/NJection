using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace NJection.Collections
{
    public interface ITreeNode<out T> : ITreeNodeBase
    {
        new T Value { get; }
        string Name { get; }
        ITreeNode<T> Current { get; }
        XElement ConfigurationElement { get; }
        IEnumerable<ITreeNode<Expression>> Nodes { get; }
        ITreeNode<Expression> ParentBlock { get; }   
    }
}
