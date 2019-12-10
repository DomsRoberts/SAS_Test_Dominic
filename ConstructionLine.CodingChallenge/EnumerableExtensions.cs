using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return (list?.Any() ?? false) == false;
        }
    }
}
