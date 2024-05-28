using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedHashSet<T> : IEnumerable<LinkedListNode<T>> where T : class
{
    private readonly Dictionary<T, LinkedListNode<T>> _dic;
    private readonly LinkedList<T> _linked;

    public int Count { get { return _linked.Count; } }

    public LinkedHashSet(int capacity = 8)
    {
        _dic = new(capacity);
        _linked = new();
    }

    public void Add(T item)
    {
        if (Contains(item))
            throw new ArgumentException();

        var node = _linked.AddLast(item);
        _dic.Add(item, node);
    }

    public void Remove(T item)
    {
        if (_dic.TryGetValue(item, out var node))
        {
            _linked.Remove(node);
            _dic.Remove(item);
        }
    }

    public bool Contains(T item)
    {
        return _dic.ContainsKey(item);
    }

    public IEnumerator<LinkedListNode<T>> GetEnumerator()
    {
        return _linked.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() 
    {        
        throw new NotImplementedException();    
    }
}
