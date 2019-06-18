namespace Wingman.Tests.Extensions
{
    using System.Collections.Generic;

    internal static class EnumerableExtensions
    {
        internal static HashSet<T> ToHashSetInternal<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }
     }
}