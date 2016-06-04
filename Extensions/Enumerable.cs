using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace NJection.Extensions
{
    public static class Enumerable
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return true;

            return !source.Any();
        }
    }
}
