using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private WorkPlan _workPlan;

    private Dictionary<RaceType, List<Unit>> _playerUnits;
    private Dictionary<RaceType, List<Unit>> _npcUnits;
    private List<IWorkable> _playerWorkableUnits;

    public Dictionary<RaceType, List<Unit>> PlayerUnits { get { return _playerUnits; } }
    public Dictionary<RaceType, List<Unit>> NPCUnits { get { return _npcUnits; } }


    private void Awake()
    {
        _playerUnits = Util.NewEnumKeyDictionary<RaceType, List<Unit>>();
        _npcUnits = Util.NewEnumKeyDictionary<RaceType, List<Unit>>();
        _playerWorkableUnits = new(8);

        UnitGenerator.Instance.RegisterGenerated(AddUnitArray);
    }

    public void Init(WorkPlan workPlan)
    {
        _workPlan = workPlan;
    }

    public void TryAddWorker(Unit unit)
    {
        if (unit is not IWorkable workable)
            return;

        _playerWorkableUnits.Add(workable);
        _workPlan.AddWaitWorker(workable);
    }

    private void AddUnitArray(Unit unit)
    {
        var race = unit.UniversalStatus.Race;
        var owner = unit.LocalStatus.Owner;

        if (owner == PropOwner.Player)
        {
            _playerUnits[race].Add(unit);
            TryAddWorker(unit);
        }
        else if (owner == PropOwner.NPC)
        {
            _npcUnits[race].Add(unit);
        }
    }
}
