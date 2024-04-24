using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviourSingleton<UnitGenerator>
{
    private Dictionary<RaceType, Queue<Unit>> _inactiveUnits;
    private UnitComponentHandler _unitPrefab;
    private Unit _newUnit;

    private IMoveSystem _workSys;

    private event Action<Unit> GeneratedUnit;

    private void Awake()
    {
        _unitPrefab = Resources.Load<UnitComponentHandler>("Prefabs/Unit");
        _inactiveUnits = Util.NewEnumKeyDictionary<RaceType, Queue<Unit>>();
    }

    public void Init(IMoveSystem moveSystem)
    {
        _workSys = moveSystem;
    }

    private void CreateNewUnit(PropOwner owner, RaceType race)
    {
        var newobj = Instantiate(_unitPrefab);
        switch (race)
        {
            case RaceType.Human:
                _newUnit = newobj.gameObject.AddComponent<Human>();
                break;
            case RaceType.Animal:
                break;
            default:
                break;
        }

        _newUnit.ComponentHandler = newobj;
        _newUnit.UnitData.SetRace(race);
        _newUnit.UnitData.SetOwner(owner);
        SetDefaultUnitData();
        _newUnit.gameObject.name = "Unit";
    }

    public UnitGenerator SetNewUnit(PropOwner owner, RaceType race)
    {
        CreateNewUnit(owner, race);
        OnGeneratedUnit();
        return this;
    }

    public UnitGenerator SetName(string name)
    {
        name ??= GetRandomName();
        _newUnit.UnitData.SetName(name);
        return this;
    }

    public UnitGenerator SetPosition(Vector2Int pos)
    {
        _newUnit.transform.position = Util.Vector2IntToVector3(pos);
        return this;
    }

    public UnitGenerator SetPosition(Vector3 pos)
    {
        _newUnit.transform.position = Util.FloorVector3(pos);
        return this;
    }

    public Unit GetNewUnit()
    {
        var temp = _newUnit;
        _newUnit = null;
        return temp;
    }

    private void SetDefaultUnitData()
    {
        _newUnit.UnitData = DataManager.Instance.GetUnitData(_newUnit.UnitData.Race);
        _newUnit.SetMoveSystem(_workSys);
    }

    private string GetRandomName()
    {
        return "랜덤";
    }

    private void OnGeneratedUnit()
    {
        GeneratedUnit?.Invoke(_newUnit);
    }

    public void RegisterGeneratedUnit(Action<Unit> action)
    {
        GeneratedUnit += action;
    }
}
