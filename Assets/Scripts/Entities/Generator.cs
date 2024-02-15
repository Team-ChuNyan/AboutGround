using System;
using System.Collections.Generic;

public abstract class Generator<T, Y> where T : Enum where Y : class
{
    protected Dictionary<T, Queue<Y>> _inactiveQueue;
    protected Y _newItem;

    public Generator()
    {
        _inactiveQueue = new();
        int enumCount = Enum.GetValues(typeof(T)).Length;

        for (int i = 0; i < enumCount; i++)
        {
            T type = (T)Enum.Parse(typeof(T), i.ToString());
            _inactiveQueue.Add(type, new());
        }
    }

    public Y GetInactiveObject(T type)
    {
        return _inactiveQueue[type].Dequeue();
    }
}
