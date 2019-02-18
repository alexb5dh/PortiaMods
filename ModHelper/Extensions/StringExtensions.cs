using System;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static bool EqualsOrdinal(this string @this, string value) => string.Equals(@this, value, StringComparison.Ordinal);

    public static string IfNullOrEmpty(this string @this, string then) => string.IsNullOrEmpty(@this) ? then : @this;

    public static string[] SplitNoEmpty(this string @this, char separator) =>
        @this.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

    private static readonly Regex SpaceCamelCaseRegex = new Regex(@"((?<=[A-Z])([A-Z])(?=[a-z]))|((?<=[a-z]+)([A-Z]))");

    public static string SpaceCamelCase(this string @this) => SpaceCamelCaseRegex.Replace(@this, @" $0");
}