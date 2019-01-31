using System;
using System.Collections.Generic;

namespace TweakIt
{
    public class SelectorComparer<T, TValue>: IComparer<T>
    {
        private readonly Func<T, TValue> _selector;
        private readonly IComparer<TValue> _valueComparer;

        public SelectorComparer(Func<T, TValue> selector, IComparer<TValue> valueComparer = null)
        {
            _selector = selector;
            _valueComparer = valueComparer ?? Comparer<TValue>.Default;
        }

        private TValue GetValue(T instance) => instance == null
            ? default(TValue)
            : _selector(instance);

        public int Compare(T x, T y) => _valueComparer.Compare(GetValue(x), GetValue(y));
    }
}