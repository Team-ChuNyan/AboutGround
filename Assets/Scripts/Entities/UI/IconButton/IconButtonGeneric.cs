using System;
using UnityEngine.UIElements;

public class IconButton<T> : IconButtonBase<Action<T>>
{
    private T _identityValue;

    public IconButton(VisualElement btn) : base(btn) { }

    public void SetIdentityValue(T value)
    {
        _identityValue = value;
    }

    protected override void OnClicked(ClickEvent evt)
    {
        Clicked?.Invoke(_identityValue);
    }
}
