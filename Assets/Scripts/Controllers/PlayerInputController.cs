using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputAction _inputAction;
    private PlayerInputAction.PlayerActions _input;

    private event Action ClickStarted;
    private event Action ClickCanceled;

    private event Action<Vector2> MovePerformed;
    private event Action<Vector2> MoveCanceled;

    private event Action<Vector2> MoveMousePerformed;

    private event Action<Vector2> PressRotationPerformed;
    private event Action<Vector2> PressRotationCancelded;

    private event Action PushRotationStarted;
    private event Action PushRotationCanceled;

    private event Action<float> PressZoomPerformed;
    private event Action<float> PressZoomCanceled;

    private event Action<float> ScrollZoomPerformed;

    private event Action EscCanceled;

    private event Action<bool> ShiftStarted;

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
        _input.Click.started += OnClickStarted;
        _input.Click.canceled += OnClickCanceled;

        _input.Move.performed += OnMovePerformed;
        _input.Move.canceled += OnMoveCanceled;


        _input.PushRotation.started += OnPushRotationStarted;
        _input.PushRotation.canceled += OnPushRotationCanceled;

        _input.MoveMouse.performed += OnMoveMousePerformed;

        _input.PressRotation.performed += OnPressRotationPerformed;
        _input.PressRotation.canceled += OnPressRotationCanceled;

        _input.ScrollZoom.performed += OnScrollZoomPerformed;

        _input.PressZoom.performed += OnPressZoomPerformed;
        _input.PressZoom.canceled += OnPressZoomCanceled;

        _input.Esc.canceled += OnEscCanceled;

        _input.Shift.started += OnShifPressed;
        _input.Shift.canceled += OnShifPressed;
    }

    #region Register Action Map
    private void OnClickStarted(InputAction.CallbackContext value)
    {
        ClickStarted?.Invoke();
    }

    private void OnClickCanceled(InputAction.CallbackContext value)
    {
        ClickCanceled?.Invoke();
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

    private void OnPushRotationStarted(InputAction.CallbackContext value)
    {
        PushRotationStarted?.Invoke();
    }

    private void OnPushRotationCanceled(InputAction.CallbackContext value)
    {
        PushRotationCanceled?.Invoke();
    }

    private void OnMoveMousePerformed(InputAction.CallbackContext value)
    {
        MoveMousePerformed?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnPressRotationPerformed(InputAction.CallbackContext value)
    {
        PressRotationPerformed?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnPressRotationCanceled(InputAction.CallbackContext value)
    {
        PressRotationCancelded?.Invoke(value.ReadValue<Vector2>());
    }

    private void OnScrollZoomPerformed(InputAction.CallbackContext value)
    {
        ScrollZoomPerformed?.Invoke(value.ReadValue<float>());
    }

    private void OnPressZoomPerformed(InputAction.CallbackContext value)
    {
        PressZoomPerformed?.Invoke(value.ReadValue<float>());
        Debug.Log(value.ReadValue<float>());
    }

    private void OnPressZoomCanceled(InputAction.CallbackContext value)
    {
        PressZoomCanceled?.Invoke(value.ReadValue<float>());
    }

    private void OnShifPressed(InputAction.CallbackContext value)
    {
        ShiftStarted?.Invoke(value.started);
    }
    #endregion

    #region Register
    public void RegisterClickStarted(Action action)
    {
        ClickStarted += action;
    }

    public void RegisterClickCanceled(Action action)
    {
        ClickCanceled += action;
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

    public void RegisterPushRotationStarted(Action action)
    {
        PushRotationStarted += action;
    }

    public void RegisterPushRotationCanceled(Action action)
    {
        PushRotationCanceled += action;
    }

    public void RegisterMoveMousePerformed(Action<Vector2> action)
    {
        MoveMousePerformed += action;
    }

    public void RegisterPressRotationPerformed(Action<Vector2> action)
    {
        PressRotationPerformed += action;
    }

    public void RegisterPressRotationCanceled(Action<Vector2> action)
    {
        PressRotationCancelded += action;
    }

    public void RegisterScrollZoomPerformed(Action<float> action)
    {
        ScrollZoomPerformed += action;
    }

    public void RegisterPressZoomPerformed(Action<float> action)
    {
        PressZoomPerformed += action;
    }

    public void RegisterPressZoomCanceled(Action<float> action)
    {
        PressZoomCanceled += action;
    }

    public void RegisterShiftPressed(Action<bool> action)
    {
        ShiftStarted += action;
    }

    // Unregister
    public void UnregisterClickStarted(Action action)
    {
        ClickStarted -= action;
    }

    public void UnregisterClickCanceled(Action action)
    {
        ClickCanceled -= action;
    }

    public void UnregisterMovePerformed(Action<Vector2> action)
    {
        MovePerformed -= action;
    }

    public void UnregisterMoveCancled(Action<Vector2> action)
    {
        MoveCanceled -= action;
    }

    public void UnregisterEsc(Action action)
    {
        EscCanceled -= action;
    }

    public void UnregisterPushRotationStarted(Action action)
    {
        PushRotationStarted -= action;
    }

    public void UnregisterPushRotationCanceled(Action action)
    {
        PushRotationCanceled -= action;
    }

    public void UnregisterMoveMousePerformed(Action<Vector2> action)
    {
        MoveMousePerformed -= action;
    }

    public void UnregisterPressRotationPerformed(Action<Vector2> action)
    {
        PressRotationPerformed -= action;
    }

    public void UnregisterPressRotationCanceled(Action<Vector2> action)
    {
        PressRotationCancelded -= action;
    }

    public void UnregisterScrollZoomPerformed(Action<float> action)
    {
        ScrollZoomPerformed -= action;
    }

    public void UnregisterPressZoomPerformed(Action<float> action)
    {
        PressZoomPerformed -= action;
    }

    public void UnregisterPressZoomCanceled(Action<float> action)
    {
        PressZoomCanceled -= action;
    }

    public void UnregisterShiftPressed(Action<bool> action)
    {
        ShiftStarted -= action;
    }
    #endregion
}
