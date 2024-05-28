using System;
using UnityEngine.UIElements;

public class IconButton : IconButtonBase<Action>
{
    public IconButton(VisualElement btn) : base(btn) { }

    protected override void OnClicked(ClickEvent evt)
    {
        Clicked?.Invoke();
    }
}
