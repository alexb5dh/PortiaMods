using System.Collections.Generic;
using System.Linq;

namespace TweakIt
{
    public static class EnumerableExtensions
    {
        public static bool In<T>(this T item, IEnumerable<T> source) => source.Contains(item);

        public static bool In<T>(this T item, params T[] source) => item.In(source.AsEnumerable());
    }
}