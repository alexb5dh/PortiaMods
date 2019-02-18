using System;
using System.Collections.Generic;

public class CustomEqualityComparer<T, TValue>: IEqualityComparer<T>
{
    private readonly Func<T, TValue> _selector;
    private readonly IEqualityComparer<TValue> _valueComparer;

    public CustomEqualityComparer(Func<T, TValue> selector, IEqualityComparer<TValue> valueComparer = null)
    {
        _selector = selector;
        _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
    }

    private TValue GetValue(T instance) => instance == null
        ? default(TValue)
        : _selector(instance);

    public bool Equals(T x, T y) => _valueComparer.Equals(GetValue(x), GetValue(y));

    public int GetHashCode(T obj) => _valueComparer.GetHashCode(GetValue(obj));
}