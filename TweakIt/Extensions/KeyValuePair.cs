using System.Collections.Generic;

namespace TweakIt
{
    public static class KeyValuePair
    {
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey,TValue>(key, value);
    }
}