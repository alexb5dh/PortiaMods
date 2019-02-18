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

    public static string StringJoin<T>(this IEnumerable<T> source, string separator) => 
        string.Join(separator, source.Select(_ => $"{_}").ToArray());

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) => new HashSet<T>(source);

    public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) =>
        source.Distinct(new CustomEqualityComparer<T, TKey>(selector));
}