using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ManagementMenuUIHandler : UIBase
{
    public enum ButtonType { Build }

    private const string ButtonName = "Button";
    private const int _count = 5;

    private List<IconButton<int>> _btns;
    private List<Action> _clickEvents;

    public ManagementMenuUIHandler(VisualElement root) : base(root) { }

    protected override void SetVisualElement()
    {
        _btns = new(_count);
        _clickEvents = new (_count);
        for (int i = 0; i < _count; i++)
        {
            var btnUI = _root.Q<VisualElement>($"{ButtonName}{i}");
            var iconBtn = new IconButton<int>(btnUI);
            iconBtn.SetIdentityValue(i);
            iconBtn.RegisterEvent(OnClickEvents);
            _btns.Add(iconBtn);
            _clickEvents.Add(null);
        }
    }

    private void OnClickEvents(int index)
    {
        _clickEvents[index]?.Invoke();
    }

    public void RegisterButtonEvent(ButtonType order, Action action)
    {
        _clickEvents[(int)order] += action;
    }
}
