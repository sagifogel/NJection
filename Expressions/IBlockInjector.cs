using System.Collections.Generic;
using System.Xml.Linq;

namespace NJection.Expressions
{
    public interface IBlockInjector
    {
        IDictionary<string, object> Definitions { get; }
        Dictionary<string, object> ResolveDefinitions(IEnumerable<XElement> definitions);
    }
}
