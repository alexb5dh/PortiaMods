using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static bool In<T>(this T item, IEnumerable<T> source) => source.Contains(item);

    public static bool In<T>(this T item, params T[] source) => item.In(source.AsEnumerable());

    public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> filter)
    {
        var index = 0;
        foreach (var element in source)
        {
            if (filter(element)) return index;
            index++;
        }

        return -1;
    }

    public static string StringJoin<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values.Select(_ => $"{_}").ToArray());
}