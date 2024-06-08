using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerInputManager _inputManager;
    private VirtualCameraController _virtualCamera;
    [SerializeField] private Transform _cameraArm;

    [Header("Move")]
    [SerializeField] private Vector2 _movementRange;
    [SerializeField] private int _bonusRange;
    [SerializeField] private float _moveSpeed = 64;
    private Vector2 _movementDirection;
    private bool _isMoveUpdating;

    [Header("Edge Scroll")]
    [SerializeField] private bool _canEdgeScroll;
    [SerializeField] private float _edgeScrollSize = 20;
    private Vector2 _edgeScrollDirection;

    [Header("Push Rotation")]
    [SerializeField] float _PushRotateSpeed = 80f;
    private const float _maxRotateX = 80;
    private const float _minRotateX = 0;
    private Vector2 _beforeMousePosition;

    [Header("Press Rotation")]
    [SerializeField] private float _rotateSpeed = 128;
    private Vector2 _rotationDirection;
    private bool _isRotationUpdating;

    [Header("Scroll Zoom")]
    [SerializeField] private float _scrollZoomSpeed = 6f;
    [SerializeField] private float _zoomSpeed = 6f;
    [SerializeField] private float _maxZoom = 50;
    [SerializeField] private float _minZoom = 1;
    private float _currentFollowOffsetZ;
    private float _targetFollowOffset;
    private bool _isZooming;

    [Header("Press Zoom")]
    [SerializeField] private float _pressZoomSpeed = 40;
    private float _pressZoomDirection;
    private bool _isPressZooming;

    private event Action CameraUpdated;

    private void Update()
    {
        CameraUpdated?.Invoke();
    }

    public void Initialize(VirtualCameraController cameraController)
    {
        _inputManager = PlayerInputManager.Instance;
        _virtualCamera = cameraController;

        _currentFollowOffsetZ = -_virtualCamera.GetBodyFollowOffset().z;
        _targetFollowOffset = _currentFollowOffsetZ;

        SetFollowObject(_cameraArm);
        SetLookAtObject(transform);
        RegisterCameraMove();

        if (_canEdgeScroll == true)
            RegisterCameraEdgeScroll();
    }

    public void SetCameraPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetCameraRotation(Vector3 dir)
    {
        _cameraArm.transform.Rotate(dir);
    }

    public void SetFollowObject(Transform target)
    {
        _virtualCamera.SetFollowTarget(target);
    }

    public void SetLookAtObject(Transform target)
    {
        _virtualCamera.SetLookAtTarget(target);
    }

    private void RegisterCameraMove()
    {
        _inputManager.RegisterMovePerformed(ChangeMovementDirection);
        _inputManager.RegisterMoveCancled(ChangeMovementDirection);

        _inputManager.RegisterPushRotationStarted(StartPushRotation);
        _inputManager.RegisterPushRotationCanceled(CancelPushRotation);

        _inputManager.RegisterPressRotationPerformed(ChangeRotateDirection);
        _inputManager.RegisterPressRotationCanceled(ChangeRotateDirection);

        _inputManager.RegisterScrollZoomPerformed(ChangeScrollZoomOffset);

        _inputManager.RegisterPressZoomPerformed(ChangePressZoomDirection);
        _inputManager.RegisterPressZoomCanceled(ChangePressZoomDirection);
    }

    private void RegisterCameraEdgeScroll()
    {
        _inputManager.RegisterMoveMousePerformed(ChangeEdgeMoveDirection);
    }

    private void ChangeMovementDirection(Vector2 dir)
    {
        _movementDirection = dir;
        ToggleMovementUpdate(dir);
    }

    private void ChangeEdgeMoveDirection(Vector2 mousePos)
    {
        if (mousePos.x < _edgeScrollSize)
            _edgeScrollDirection.x = -1;
        else if (mousePos.x > Screen.width - _edgeScrollSize)
            _edgeScrollDirection.x = +1;
        else
            _edgeScrollDirection.x = 0;

        if (mousePos.y < _edgeScrollSize)
            _edgeScrollDirection.y = -1;
        else if (mousePos.y > Screen.height - _edgeScrollSize)
            _edgeScrollDirection.y = +1;
        else
            _edgeScrollDirection.y = 0;

        ToggleMovementUpdate(_edgeScrollDirection);
    }

    private void ToggleMovementUpdate(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            CameraUpdated -= Move;
            _isMoveUpdating = false;
        }
        else if (_isMoveUpdating == false)
        {
            CameraUpdated += Move;
            _isMoveUpdating = true;
        }
    }

    private void Move()
    {
        float deltaTimeSpeed = _moveSpeed * Time.deltaTime;

        Vector3 forward = _cameraArm.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _cameraArm.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDir = (_movementDirection.y + _edgeScrollDirection.y) * deltaTimeSpeed * forward
                        + (_movementDirection.x + _edgeScrollDirection.x) * deltaTimeSpeed * right;

        transform.position += moveDir;
    }

    private void ChangeRotateDirection(Vector2 dir)
    {
        _rotationDirection = dir;
        ToggleRotationUpdate();
    }

    private void ToggleRotationUpdate()
    {
        if (_rotationDirection == Vector2.zero
         && _isRotationUpdating == true)
        {
            CameraUpdated -= UpdateRotation;
            _isRotationUpdating = false;
        }
        else if (_isRotationUpdating == false)
        {
            CameraUpdated += UpdateRotation;
            _isRotationUpdating = true;
        }
    }

    private void UpdateRotation()
    {
        Vector3 rotation = _rotateSpeed * _rotationDirection * Time.deltaTime;
        Vector3 currentRotation = _cameraArm.localEulerAngles + rotation;
        currentRotation.x = Mathf.Clamp(currentRotation.x, _minRotateX, _maxRotateX);

        _cameraArm.localEulerAngles = currentRotation;
    }

    private void StartPushRotation()
    {
        _inputManager.RegisterMoveMousePerformed(SetBeforeMousePosition);
        _inputManager.RegisterMoveMousePerformed(ChangePushRotateDirection);
    }

    private void SetBeforeMousePosition(Vector2 pos)
    {
        _beforeMousePosition = pos;
        _inputManager.UnregisterMoveMousePerformed(SetBeforeMousePosition);
    }

    private void ChangePushRotateDirection(Vector2 pos)
    {
        Vector3 dir = _PushRotateSpeed * Time.deltaTime * (pos - _beforeMousePosition).normalized;
        Vector3 rotateDir = _cameraArm.localEulerAngles;

        rotateDir.x -= dir.y;
        rotateDir.x = Mathf.Clamp(rotateDir.x, _minRotateX, _maxRotateX);
        rotateDir.y += dir.x;

        _cameraArm.localEulerAngles = rotateDir;
        _beforeMousePosition = pos;
    }

    private void CancelPushRotation()
    {
        _inputManager.UnregisterMoveMousePerformed(ChangePushRotateDirection);
    }

    private void ChangeScrollZoomOffset(float value)
    {
        float targetValue = _targetFollowOffset - (value * _scrollZoomSpeed);
        _targetFollowOffset = Mathf.Clamp(targetValue, _minZoom, _maxZoom);

        if (_isZooming == false)
        {
            CameraUpdated += UpdateZoom;
            _isZooming = true;
        }
    }

    private void ChangePressZoomDirection(float value)
    {
        _pressZoomDirection = value;

        if (value != 0 && _isPressZooming == false)
        {
            _isPressZooming = true;
            CameraUpdated += UpdatePressZoom;

        }
        else if (_isPressZooming == true && value == 0)
        {
            _isPressZooming = false;
            CameraUpdated -= UpdatePressZoom;
        }
    }

    private void UpdatePressZoom()
    {
        float nextOffsetZ = _targetFollowOffset + _pressZoomDirection * Time.deltaTime * _pressZoomSpeed;
        nextOffsetZ = Mathf.Clamp(nextOffsetZ, _minZoom, _maxZoom);

        _targetFollowOffset = nextOffsetZ;
        _currentFollowOffsetZ = nextOffsetZ;
        _virtualCamera.SetFollowOffset(0, 0, -_currentFollowOffsetZ);

    }

    private void UpdateZoom()
    {
        _currentFollowOffsetZ = Mathf.Lerp(_currentFollowOffsetZ, _targetFollowOffset, _zoomSpeed * Time.deltaTime);

        if (Mathf.Abs(_currentFollowOffsetZ - _targetFollowOffset) < 0.01f)
        {
            _virtualCamera.SetFollowOffset(0, 0, -_targetFollowOffset);
            CameraUpdated -= UpdateZoom;
            _isZooming = false;
        }
        else
        {
            _virtualCamera.SetFollowOffset(0, 0, -_currentFollowOffsetZ);
        }
    }
}
