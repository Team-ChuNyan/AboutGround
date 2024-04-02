using System.Collections.Generic;
using UnityEngine.UIElements;

public class InteractionMenuUIHandler : UIBase
{
    public readonly InteractionViewModel ViewModel;
    private const string ButtonName = "Button";
    private const int _count = 4;

    private List<IconButton> _btns;

    public InteractionMenuUIHandler(VisualElement root) : base(root) 
    {
        Hide();
        ViewModel = new InteractionViewModel();
        ViewModel.RegisterChangedSelection(RefreshButtons);
    }

    protected override void SetVisualElement()
    {
        _btns = new(_count);
        for (int i = 0; i < _count; i++)
        {
            var btnUI = _root.Q<VisualElement>($"{ButtonName}{i}");
            var iconBtn = new IconButton(btnUI);
            iconBtn.SetIndex(i);
            iconBtn.RegisterEvent(StartInteractionEvent);

            _btns.Add(iconBtn);
        }
    }

    public void RefreshButtons()
    {
        var types = ViewModel.InteractionTypes;
        int typeCount = types.Count;

        if (typeCount > 0)
            Show();
        else
            Hide();

        for (int i = 0; i < _btns.Count; i++)
        {
            if (typeCount <= i )
            {
                _btns[i].Hide();
                continue;
            }
            _btns[i].Show();
            _btns[i].SetText(types[i].ToString());
            //_btns[i].SetSprite() TODO : 이미지 변경
        }
    }

    private void StartInteractionEvent(int index)
    {
        InteractionType type = ViewModel.InteractionTypes[index];
        ViewModel.Events[type]?.Invoke();
    }
}
