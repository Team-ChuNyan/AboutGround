using UnityEngine;
using UnityEngine.InputSystem;

public class BlueprintConstructing
{
    private int _buildArea;
    private PlayerInputController _controller;
    private Camera _cam;
    private Building _blueprint;
    private bool _isStarting;

    private const int MaxRange = 1000;

    public BlueprintConstructing()
    {
        _cam = Camera.main;
        _buildArea = 1 << 7;
    }

    public void Init(PlayerInputController con)
    {
        _controller = con;
    }

    public void StartConstruction(BuildingType type)
    {
        if (_isStarting == true) return;

        var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 worldPos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRange, _buildArea))
        {
            worldPos = hitInfo.collider.transform.position;
            worldPos.y++;
        }

        _blueprint = BuildingGenerator.Instance.SetNewBuilding(type)
                                               .ChangeBlueprintMode()
                                               .SetPosition(worldPos)
                                               .GenerateBuilding();

        _controller.RegisterMoveMousePerformed(RaycastMousePosition);
        _isStarting = true;
    }

    private void RaycastMousePosition(Vector2 pos)
    {
        var ray = _cam.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, MaxRange, _buildArea))
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

    private void Construct()
    {
        //_blueprint

        // 클릭하면 건설이 되고 업무에 건설이 할당 되도록
    }
}
