using System.Collections.Generic;

public class SelectPropController : ICancelable
{
    private PlayerInputManager _inputManager;
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

    public void Init(QuickCanceling quickCanceling)
    {
        _inputManager = PlayerInputManager.Instance;
        _quickCanceling = quickCanceling;
    }

    public void InitObjectSelecting(InteractionViewModel model,SelectionBoxUIHandler selectionBoxUI, PropsContainer props, MouseInputHandler inputHandler)
    {
        _interactionViewModel = model;
        _selecting.Init(selectionBoxUI, _quickCanceling, props);
        _selecting.InitInput(_inputManager, inputHandler);
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
