using System.Collections.Generic;
using System.Linq;

namespace FBDropshipper.Common.Extensions
{
    public static class ListExtension
    {
        public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }
    }
}