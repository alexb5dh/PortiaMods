using System;
using System.Collections.Generic;
using System.Linq;

namespace TweakIt
{
    public static class String
    {
        public static bool EqualsOrdinal(this string @this, string value) => string.Equals(@this, value, StringComparison.Ordinal);

        public static string Join<T>(string separator, IEnumerable<T> values) => string.Join(separator, values.Select(_ => $"{_}").ToArray());

        public static string IfNullOrEmpty(this string @this, string then) => string.IsNullOrEmpty(@this) ? then : @this;
    }
}