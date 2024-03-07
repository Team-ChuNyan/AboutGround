using System;
using UnityEngine;
using UnityEngine.UIElements;

public class IconButton<T> : IDynamicTypeUpdater<T> where T : Enum
{
    private VisualElement _button;
    private event Action _clickedEventHandler;

    public IconButton(VisualElement btn)
    {
        _button = btn;
        _button.RegisterCallback<ClickEvent>(OnClicked);
    }

    private void OnClicked(ClickEvent evt)
    {
        _clickedEventHandler?.Invoke();
    }

    public void RegisterEvent(Action action)
    {
        _clickedEventHandler += action;
    }

    public void UnregisterEvent(Action action)
    {
        _clickedEventHandler -= action;
    }

    public void ClearEvent()
    {
        _clickedEventHandler = null;
    }

    public void UpdateFormType(T resourcesType)
    {
        Debug.Log(resourcesType);
    }
}
