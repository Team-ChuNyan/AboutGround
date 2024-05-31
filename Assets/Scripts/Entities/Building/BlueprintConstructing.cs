using UnityEngine;
using UnityEngine.InputSystem;

public class BlueprintConstructing : ICancelable
{
    private Camera _cam;
    private MouseInputHandler _inputHandler;
    private QuickCanceling _quickCanceling;
    private Building _blueprint;

    private int _buildRayer;
    private bool _isStarting;

    private const int MaxRange = 1000;

    public BlueprintConstructing()
    {
        _cam = Camera.main;
        _buildRayer = 1 << 7;
    }

    public void Init(MouseInputHandler inputHandler, QuickCanceling quickCanceling)
    {
        _inputHandler = inputHandler;
        _quickCanceling = quickCanceling;

        _inputHandler.RegisterMoveMousePerformed(MouseInputHandler.LeftClick.Constructing, RaycastMousePosition);
        _inputHandler.RegisterClickCanceled(MouseInputHandler.LeftClick.Constructing, MosePointConstruct);
    }

    public void Start(BuildingType type)
    {
        if (_isStarting == true)
            return;

        var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 worldPos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRange, _buildRayer))
        {
            worldPos = hitInfo.collider.transform.position;
            worldPos.y++;
        }

        _blueprint = BuildingGenerator.Instance.Prepare(type)
                                               .ConvertBlueprint()
                                               .SetPosition(worldPos)
                                               .Generate();

        _inputHandler.ChangeLeftClickMode(MouseInputHandler.LeftClick.Constructing);
        _quickCanceling.Push(this);
        _isStarting = true;
    }

    public void Cancel()
    {
        _isStarting = false;
        _quickCanceling.Remove(this);
        _blueprint.Destroy();
        _inputHandler.ChangeDefaultMode();
    }

    private void RaycastMousePosition(Vector2 pos)
    {
        var ray = _cam.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRange, _buildRayer))
        {
            var worldPos = hitInfo.collider.transform.position;
            worldPos.y++;
            UpdateBuildingPosition(worldPos);
        }
    }

    private void UpdateBuildingPosition(Vector3 pos)
    {
        _blueprint.transform.position = pos;
    }

    private void MosePointConstruct()
    {
        if (PlayerInputController.IsPointerOverUI == true)
            return;

        BuildingGenerator.Instance.Prepare(_blueprint.GlobalStatus.BuildingType)
                                  .ConvertBlueprint()
                                  .SetPosition(_blueprint.transform.position)
                                  .Generate();

        // TODO : 꾹 클릭시 연속적으로 배치하는 법
        // clickStarted 할 때 MoveMouse에 계속 건설하도록 등록
        // Pathnode에 지을 수 없는 처리가 필요함
    }

    public void QuickCancel()
    {
        Cancel();
    }

    public bool IsCanceled()
    {
        return _isStarting == false;
    }
}
