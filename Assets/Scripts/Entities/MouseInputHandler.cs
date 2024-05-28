using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputHandler
{
    public enum LeftClick { Selecting, Constructing }

    private PlayerInputController _inputController;

    private LeftClick _default;
    private LeftClick _mode;

    private readonly Dictionary<LeftClick, Action> _clickStartedActions;
    private readonly Dictionary<LeftClick, Action> _clickCanceledActions;
    private readonly Dictionary<LeftClick, Action<Vector2>> _mouseMovePerformedActions;

    public MouseInputHandler()
    {
        _clickStartedActions = new();
        _clickCanceledActions = new();
        _mouseMovePerformedActions = new();
    }

    public void Init(PlayerInputController con)
    {
        _inputController = con;
    }

    public void ChangeLeftClickMode(LeftClick mode)
    {
        _inputController.UnregisterClickStarted(TryGetAction(_clickStartedActions, _mode));
        _inputController.UnregisterClickCanceled(TryGetAction(_clickCanceledActions, _mode));
        _inputController.UnregisterMoveMousePerformed(TryGetAction(_mouseMovePerformedActions, _mode));

        _inputController.RegisterClickStarted(TryGetAction(_clickStartedActions, mode));
        _inputController.RegisterClickCanceled(TryGetAction(_clickCanceledActions, mode));
        _inputController.RegisterMoveMousePerformed(TryGetAction(_mouseMovePerformedActions, mode));

        _mode = mode;
    }

    private T TryGetAction<T>(Dictionary<LeftClick, T> dic, LeftClick mode) where T : Delegate
    {
        dic.TryGetValue(mode, out var action);
        return action;
    }

    public void ChangeDefaultMode()
    {
        ChangeLeftClickMode(_default);
    }

    public void RegisterClickStarted(LeftClick mode, Action action)
    {
        if (_clickStartedActions.ContainsKey(mode) == false)
            _clickStartedActions[mode] = null;

        _clickStartedActions[mode] += action;
    }

    public void RegisterClickCanceled(LeftClick mode, Action action)
    {
        if (_clickCanceledActions.ContainsKey(mode) == false)
            _clickCanceledActions[mode] = null;

        _clickCanceledActions[mode] += action;
    }

    public void RegisterMoveMousePerformed(LeftClick mode, Action<Vector2> action)
    {
        if (_mouseMovePerformedActions.ContainsKey(mode) == false)
            _mouseMovePerformedActions[mode] = null;

        _mouseMovePerformedActions[mode] += action;
    }
}
