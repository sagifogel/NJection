
namespace NJection.Collections
{
    public interface ITreeNodeBase
    {
        object Value { get; }
        ExpressionRoot Root { get; }
        ITreeNodeBase Parent { get; }
    }
}
