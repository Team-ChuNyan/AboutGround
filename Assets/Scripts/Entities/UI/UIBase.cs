using UnityEngine.UIElements;

public abstract class UIBase
{
    protected VisualElement _root;

    public UIBase(VisualElement root)
    {
        _root = root;
        SetVisualElement();
    }

    protected virtual void SetVisualElement()
    {

    }

    protected virtual void Show()
    {
        _root.style.display = DisplayStyle.Flex;
    }

    protected virtual void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }
}
