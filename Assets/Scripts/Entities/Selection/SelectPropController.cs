using System.Collections.Generic;

public class SelectPropController : ICancelable
{
    private PlayerInputController _inputController;
    private PropSelecting _selecting;
    private QuickCanceling _quickCanceling;
    private InteractionViewModel _interactionViewModel;

    private List<InteractionType> _currentTypes;

    public SelectPropController()
    {
        _currentTypes = new();
        _selecting = new();
        _selecting.RegisterSelectionChanged(ToggleQuickCanceling);
    }

    public void Init(PlayerInputController inputController, QuickCanceling quickCanceling)
    {
        _inputController = inputController;
        _quickCanceling = quickCanceling;
    }

    public void InitObjectSelecting(InteractionViewModel model,SelectionBoxUIHandler selectionBoxUI, PropsContainer props)
    {
        _interactionViewModel = model;
        _selecting.Init(_inputController, selectionBoxUI, _quickCanceling, props);
        model.Init(_selecting.CurrentSelection, _currentTypes);
    }

    public bool IsCanceled()
    {
        return _selecting.CurrentSelection.Count == 0;
    }

    public void QuickCancel()
    {
        _selecting.UnselectAllSelection();
    }

    private void ToggleQuickCanceling()
    {
        if (_selecting.CurrentSelection.Count > 0)
        {
            _quickCanceling.Push(this);
            ChangeCurrentInteractionTypes();
        }
        else
        {
            _quickCanceling.Remove(this);
        }

        _interactionViewModel.OnChangedSelection();
    }

    private void ChangeCurrentInteractionTypes()
    {
        _currentTypes.Clear();
        List<InteractionType> defaultType = null;
        switch (_selecting.SelectType)
        {
            case SelectPropType.PlayerUnit:
                defaultType = Unit.PlayerUnitInteraction;
                break;
            case SelectPropType.NPC:
                defaultType = Unit.PlayerUnitInteraction;
                break;
            case SelectPropType.Pack:
                defaultType = Pack.DefaultInteraction;
                break;
            case SelectPropType.None:
            default:
                UnityEngine.Debug.Log("RefreshInteractionUI");
                break;
        }

        foreach (InteractionType type in defaultType)
        {
            _currentTypes.Add(type);
        }
    }
}
