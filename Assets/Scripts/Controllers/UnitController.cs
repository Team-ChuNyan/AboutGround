using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private IMoveSystem _groundPathfinding;
    private UnitGenerator _unitGenerator;
    public List<Unit> PlayerUnit;
    public List<Unit> NpcUnit;
    public List<IWorkable> WorkablePlayerUnit;

    private void Awake()
    {
        _unitGenerator = gameObject.AddComponent<UnitGenerator>();
        PlayerUnit = new List<Unit>(8);
        NpcUnit = new List<Unit>(16);
        WorkablePlayerUnit = new List<IWorkable>(8);
    }

    public Unit CreateNewPlayerUnit(RaceType race, string name = null)
    {
        var unit = _unitGenerator
            .SetNewUnit(race)
            .SetName(name)
            .SetMoveSystem(_groundPathfinding)
            .GetNewUnit();

        AddPlayerUnitList(unit);
        return unit;
    }


    private void AddPlayerUnitList(Unit unit)
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

    public void SetGroundPathFinding(GroundPathfinding groundPathfinding)
    {
        _groundPathfinding = groundPathfinding;
    }
}
