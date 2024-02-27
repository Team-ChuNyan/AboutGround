using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputAction _inputAction;
    private PlayerInputAction.PlayerActions _input;

    private event Action<Vector2> MoveStarted;
    private event Action<Vector2> MovePerformed;
    private event Action<Vector2> MoveCanceled;
    private event Action EscCanceled;

    private event Action Updated;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _input = _inputAction.Player;
        RegisterInputCallback();
    }

    private void Update()
    {
        Updated?.Invoke();
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
        _input.Move.performed += OnMovePerformed;
        _input.Move.canceled += OnMoveCanceled;
        _input.Esc.canceled += OnEscCanceled;
    }

    #region Register Action Map
    private void OnMoveStarted(InputAction.CallbackContext value)
    {
        MoveStarted?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnMovePerformed(InputAction.CallbackContext value)
    {
        MovePerformed?.Invoke(value.ReadValue<Vector2>());
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
    public void RegisterUpdate(Action action)
    {
        Updated += action;
    }

    public void RegisterMoveStarted(Action<Vector2> action)
    {
        MoveStarted += action;
    }

    public void RegisterMovePerformed(Action<Vector2> action) 
    {
        MovePerformed += action;
    }

    public void RegisterMoveCancled(Action<Vector2> action)
    {
        MoveCanceled += action;
    }

    public void RegisterEsc(Action action)
    {
        EscCanceled += action;
    }
    #endregion

    #region Unregister
    public void UnregisterUpdate(Action action)
    {
        Updated -= action;
    }

    public void UnregisterMoveStarted(Action<Vector2> action)
    {
        MoveStarted -= action;
    }

    public void UnregisterMovePerformed(Action<Vector2> action)
    {
        MovePerformed -= action;
    }

    public void UnregisterMoveCancled(Action<Vector2> action)
    {
        MoveCanceled -= action;
    }

    public void UnregisterMovePerformed(Action action)
    {
        Updated -= action;
    }

    public void UnregisterEsc(Action action)
    {
        EscCanceled -= action;
    }
    #endregion
}
