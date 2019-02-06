using System;

public static class StringExtensions
{
    public static bool EqualsOrdinal(this string @this, string value) => string.Equals(@this, value, StringComparison.Ordinal);

    public static string IfNullOrEmpty(this string @this, string then) => string.IsNullOrEmpty(@this) ? then : @this;
}