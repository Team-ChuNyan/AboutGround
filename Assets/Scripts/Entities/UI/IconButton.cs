using System;
using UnityEngine;
using UnityEngine.UIElements;

public class IconButton : UIBase
{
    private VisualElement _button;
    private VisualElement _icon;
    private Label _text;

    private int _index;

    private event Action<int> _clickedEventHandler;

    public IconButton(VisualElement btn) : base(btn)
    {
        _button = btn;
        _icon = btn.Q<VisualElement>("Icon");
        _text = btn.Q<Label>("Text");
        _button.RegisterCallback<ClickEvent>(OnClicked);
    }

    private void OnClicked(ClickEvent evt)
    {
        _clickedEventHandler?.Invoke(_index);
    }

    public void RegisterEvent(Action<int> action)
    {
        _clickedEventHandler += action;
    }

    public void UnregisterEvent(Action<int>  action)
    {
        _clickedEventHandler -= action;
    }

    public void ClearEvent()
    {
        _clickedEventHandler = null;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetSprite(Sprite sprite)
    {
        _icon.style.backgroundImage = new StyleBackground(sprite);
    }
}
