using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // TO DO : 인풋 시스템에 할당과 해제를 주기적으로 해야할 경우가 있는가?
    private PlayerInputAction _inputAction;
    private PlayerInputAction.PlayerActions _input;

    private event Action<Vector2> MoveStarted;
    private event Action<Vector2> MoveCanceled;
    private event Action EscCanceled;
    
    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _input = _inputAction.Player;
        RegisterInputCallback();
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void RegisterInputCallback()
    {
        _input.Move.started += OnMoveStarted;
        _input.Move.canceled += OnMoveCanceled;
        _input.Esc.canceled += OnEscCanceled;
    }

    #region Event
    private void OnMoveStarted(InputAction.CallbackContext value)
    {
        MoveStarted?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext value)
    {
        MoveCanceled?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnEscCanceled(InputAction.CallbackContext value)
    {
        EscCanceled?.Invoke();
    }
    #endregion

    #region Register
    public void RegisterMove(Action<Vector2> action)
    {
        MoveStarted += action;
        MoveCanceled += action;
    }

    public void UnregisterMove(Action<Vector2> action)
    {
        MoveStarted -= action;
        MoveCanceled -= action;
    }

    public void RegisterEsc(Action action)
    {
        EscCanceled += action;
    }

    public void UnregisterEsc(Action action)
    {
        EscCanceled -= action;
    }
    #endregion
}
