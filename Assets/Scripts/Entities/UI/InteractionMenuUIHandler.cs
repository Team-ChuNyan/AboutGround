using System.Collections.Generic;
using UnityEngine.UIElements;

public class InteractionMenuUIHandler : UIBase
{
    private const string ButtonName = "Button";
    private const int _count = 4;

    private List<IDynamicTypeUpdater<InteractionType>> _btns;
    private EnumViewModel<InteractionType> _interactionBtnViewModel;


    public InteractionMenuUIHandler(VisualElement root) : base(root) 
    {
        _interactionBtnViewModel = new EnumViewModel<InteractionType>();
        _interactionBtnViewModel.RegisterValueChanged(ChangeButtons);
    }

    protected override void SetVisualElement()
    {
        _btns = new(_count);
        for (int i = 0; i < _count; i++)
        {
            var btn = _root.Q<VisualElement>($"{ButtonName}{i}");
            _btns.Add(new IconButton<InteractionType>(btn));
        }
    }

    public EnumViewModel<InteractionType> GetViewModel()
    {
        return _interactionBtnViewModel;
    }

    private void ChangeButtons()
    {
        var list = _interactionBtnViewModel.ActionList;
        for (int i = 0; i < list.Count; i++)
        {
            _btns[i].UpdateFormType(list[i]);
        }
    }
}
