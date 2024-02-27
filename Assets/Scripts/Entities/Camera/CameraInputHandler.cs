using UnityEngine;

public class CameraInputHandler : MonoBehaviour
{
    private PlayerInputController _controller;
    private Vector2 _moveDir;
    private float _moveSpeed = 2;

    public void Initialize(PlayerInputController controller)
    {
        _controller = controller;
        RegisterCameraMove();
    }

    private void MoveCamera()
    {
        transform.position += new Vector3(_moveDir.x * _moveSpeed, 0, _moveDir.y * _moveSpeed);
    }

    public void RegisterCameraMove()
    {
        _controller.RegisterMoveStarted(GetInputDirection);
        _controller.RegisterMovePerformed(GetInputDirection);
        _controller.RegisterMoveCancled(GetInputDirection);
    }

    public void GetInputDirection(Vector2 input)
    {
        _moveDir = input;
        ManageUpdateEvent(input);
        Debug.Log(input);
    }

    private void ManageUpdateEvent(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            _controller.UnregisterMovePerformed(MoveCamera);
        }
        else
        {
            _controller.UnregisterMovePerformed(MoveCamera);
            _controller.RegisterUpdate(MoveCamera);
        }
    }


}
