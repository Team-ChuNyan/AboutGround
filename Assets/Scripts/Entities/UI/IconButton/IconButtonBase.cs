using System;
using UnityEngine.UIElements;
using UnityEngine;

public abstract class IconButtonBase<T> : UIBase where T : Delegate
{
    protected VisualElement _button;
    protected VisualElement _icon;
    protected Label _text;

    protected T Clicked;

    public IconButtonBase(VisualElement btn) : base(btn)
    {
        _button = btn;
        _icon = btn.Q<VisualElement>("Icon");
        _text = btn.Q<Label>("Text");
        _button.RegisterCallback<ClickEvent>(OnClicked);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetSprite(Sprite sprite)
    {
        _icon.style.backgroundImage = new StyleBackground(sprite);
    }

    protected abstract void OnClicked(ClickEvent evt);

    public void ClearEvent()
    {
        Clicked = null;
    }

    public void RegisterEvent(T action)
    {
        Clicked = (T)Delegate.Combine(Clicked, action);
    }

    public void UnregisterEvent(T action)
    {
        Clicked = (T)Delegate.Remove(Clicked, action);
    }
}
