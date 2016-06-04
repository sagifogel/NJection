using System.Linq.Expressions;

namespace NJection.Collections
{
    public interface IGenericInjector<T> where T : class
    {       
        T Parse();
        ExpressionType Type { get; }
    }
}
