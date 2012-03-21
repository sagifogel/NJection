using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJection.Collections
{
    public interface ITreeNode<out T> : ITreeNode
    {
        T Value { get; }
        IEnumerable<ITreeNode<T>> Nodes { get; }
    }
}
