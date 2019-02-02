using System;

namespace DebugMod.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsOrdinal(this string @this, string value) => string.Equals(@this, value, StringComparison.Ordinal);

        public static string[] Split(this string @this, char separator, StringSplitOptions options) => @this.Split(new[] { separator }, options);

        public static string[] Split(this string @this, StringSplitOptions options) => @this.Split((char[])null, options);
    }
}