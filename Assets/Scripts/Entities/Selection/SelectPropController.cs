using System.Collections.Generic;

public class SelectPropController : ICancelable
{
    private PlayerInputController _inputController;
    private PropSelecting _selecting;
    private QuickCanceling _quickCanceling;
    private HashSet<ISelectable> _selections;

    public SelectPropController()
    {
        _selecting = new();
        _selections = _selecting.GetSelection();

        _selecting.RegisterSelectionChanged(ToggleRegisterQuickCancel);
    }

    public void Init(PlayerInputController inputController, QuickCanceling quickCanceling)
    {
        _inputController = inputController;
        _quickCanceling = quickCanceling;
    }

    public void InitObjectSelecting(SelectionBoxUIHandler selectionBoxUI, PropsContainer props)
    {
        _selecting.Init(_inputController, selectionBoxUI, _quickCanceling, props);
    }

    public bool IsCanceled()
    {
        return _selections.Count == 0;
    }

    public void QuickCancel()
    {
        UnselectAllSelection();
    }

    private void UnselectAllSelection()
    {
        foreach (var item in _selections)
        {
            item.CancelSelection();
        }
        _selections.Clear();
    }

    private void ToggleRegisterQuickCancel()
    {
        if (_selections.Count > 0)
            _quickCanceling.Push(this);
        else
            _quickCanceling.Remove(this);
    }
}
