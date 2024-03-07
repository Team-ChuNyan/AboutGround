using System.Collections.Generic;
using System;

public class EnumViewModel<T> where T : Enum
{
    private List<T> _Values;

    private event Action ValueChanged;

    public List<T> ActionList
    {
        get { return _Values; }
        set
        {
            _Values = value;
            ValueChanged?.Invoke();
        }
    }

    public void RegisterValueChanged(Action action)
    {
        ValueChanged += action;
    }

    public void UnregisterActionListChanged(Action action)
    {
        ValueChanged -= action;
    }
}
