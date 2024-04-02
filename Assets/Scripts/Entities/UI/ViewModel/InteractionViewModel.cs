using System;
using System.Collections.Generic;

public class InteractionViewModel
{
    private readonly Dictionary<InteractionType, Action> _events;
    private List<ISelectable> _selections;
    private List<InteractionType> _interactionTypes;

    public Dictionary<InteractionType, Action> Events { get { return _events; } }
    public List<ISelectable> Selections { get { return _selections; } }
    public List<InteractionType> InteractionTypes { get { return _interactionTypes; } }

    private event Action ChangedSelection;

    public InteractionViewModel()
    {
        _events = new();
    }

    public void Init(List<ISelectable> selections, List<InteractionType> types)
    {
        _selections = selections;
        _interactionTypes = types;
    }

    public void RegisterEvent(InteractionType type, Action action)
    {
        _events[type] += action;
    }

    public void RegisterChangedSelection(Action action)
    {
        ChangedSelection += action;
    }

    public void OnChangedSelection()
    {
        ChangedSelection?.Invoke();
    }
}
