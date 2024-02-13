using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public Pathfinding Pathfinding;

    private UnitGenerator _unitGenerator;
    public List<Unit> PlayerUnit;
    public List<Unit> NpcUnit;
    public List<IWorkable> WorkablePlayerUnit;

    private void Awake()
    {
        _unitGenerator = gameObject.AddComponent<UnitGenerator>();
        Pathfinding = new Pathfinding();
        PlayerUnit = new List<Unit>(8);
        NpcUnit = new List<Unit>(16);
        WorkablePlayerUnit = new List<IWorkable>(8);
    }

    public Unit CreateNewPlayerUnit(RaceType race, string name = null)
    {
        var unit = _unitGenerator
            .SetNewUnit(race)
            .SetName(name)
            .GetNewUnit();

        AddPlayerUnitList(unit);
        return unit;
    }

    public void MoveUnit(Unit unit, Vector2Int goal)
    {
        if (unit is not IMovable move)
            return;

        var movementPath = move.GetMovementPath();
        var currentPos = move.GetCurrentPosition();
        Pathfinding.ReceiveMovementPath(movementPath,currentPos, goal);
        move.Move();
    }

    {
        PlayerUnit.Add(unit);
        if (unit is IWorkable workable)
        {
            WorkablePlayerUnit.Add(workable);
        }
    }

    private void RemovePlayerUnitList(Unit unit)
    {
        // TODO : 기능 추가
        PlayerUnit.Remove(unit);
        if (unit is IWorkable workable)
        {
            WorkablePlayerUnit.Remove(workable);
        }
    }
}