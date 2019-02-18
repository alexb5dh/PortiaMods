using System;
using System.Collections.Generic;

public class CustomComparer<T, TValue>: IComparer<T>
{
    private readonly Func<T, TValue> _selector;
    private readonly IComparer<TValue> _valueComparer;

    public CustomComparer(Func<T, TValue> selector, IComparer<TValue> valueComparer = null)
    {
        _selector = selector;
        _valueComparer = valueComparer ?? Comparer<TValue>.Default;
    }

    private TValue GetValue(T instance) => instance == null
        ? default(TValue)
        : _selector(instance);

    public int Compare(T x, T y) => _valueComparer.Compare(GetValue(x), GetValue(y));
}