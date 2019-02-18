using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Multimap<TKey, TValue>: IEnumerable<KeyValuePair<TKey, HashSet<TValue>>>, IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly Dictionary<TKey, HashSet<TValue>> _dictionary = new Dictionary<TKey, HashSet<TValue>>();

    public bool Add(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out var values))
            _dictionary[key] = values = new HashSet<TValue>();
        return values.Add(value);
    }

    public void Add(TKey key, IEnumerable<TValue> values)
    {
        foreach (var value in values) Add(key, value);
    }

    public IEnumerable<TValue> GetValues(TKey key) => _dictionary.TryGetValue(key, out var values) ? (IEnumerable<TValue>)values : new TValue[0];

    public bool Remove(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out var values)) return false;
        var isRemoved = values.Remove(value);
        if (isRemoved && !values.Any()) _dictionary.Remove(key);
        return isRemoved;
    }

    public bool RemoveKey(TKey key) => _dictionary.Remove(key);

    public void Clear() => _dictionary.Clear();

    #region Implementation of IEnumerable

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        _dictionary.SelectMany(p => p.Value, (p, v) => KeyValuePair.Create(p.Key, v)).GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, HashSet<TValue>>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary).GetEnumerator();

    #endregion

}