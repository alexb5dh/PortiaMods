using System;
using System.Collections.Generic;

namespace Hotkeys.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, Func<TValue> valueFactory)
        {
            if (@this.ContainsKey(key)) return false;
            try
            {
                @this.Add(key, valueFactory());
                return true;
            }
            catch (ArgumentException) { return false; }
        }
    }
}