using System.Collections.Generic;

public class MultiKeyDictionary<TKey1, TKey2, Value>
{
    private Dictionary<TKey1, Dictionary<TKey2, Value>> _dictionary;

    public MultiKeyDictionary() 
    {
        _dictionary = new();
    }

    public Value this[TKey1 key1, TKey2 key2]
    {
        get
        {
            return _dictionary[key1][key2];
        }
        set
        {
            _dictionary[key1][key2] = value;
        }
    }

    public Dictionary<TKey2, Value> this[TKey1 key1]
    {
        get
        {
            return _dictionary[key1];
        }
        set
        {
            _dictionary[key1] = value;
        }
    }

    public void Add(TKey1 key1, TKey2 key2, Value value)
    {
        _dictionary[key1].Add(key2, value);
        this[key1, key2] = value;
    }

    public void Remove(TKey1 key)
    {
        _dictionary.Remove(key);
    }

    public void Remove(TKey1 key1, TKey2 key2)
    {
        _dictionary[key1].Remove(key2);
    }

    public void Remove(TKey1 key1, TKey2 key2, out Value value)
    {
        _dictionary[key1].Remove(key2, out value);
    }

    public void Clear()
    {
        foreach (var item in _dictionary)
        {
            item.Value.Clear();
        }
        _dictionary.Clear();
    }

    public bool ContainsKey(TKey1 key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool ContainsKey(TKey1 key1, TKey2 key2)
    {
        if (_dictionary.ContainsKey(key1) == false)
        {
            return false;
        }

        return _dictionary[key1].ContainsKey(key2);
    }
}
