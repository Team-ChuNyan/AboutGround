using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviourSingleton<UnitGenerator>, IObjectGenerator<Unit>
{
    private UnitComponentHandler _unitPrefab;

    private Dictionary<RaceType, Queue<Unit>> _inactives;
    private Transform _root;
    private Unit _newUnit;

    private static IMoveSystem _workSys;

    private event Action<Unit> Generated;
    private event Action<Unit> Destroyed;

    private void Awake()
    {
        _unitPrefab = Resources.Load<UnitComponentHandler>("Prefabs/Unit");
        _root = new GameObject("Units").transform;
        _inactives = Util.NewEnumKeyDictionary<RaceType, Queue<Unit>>();
    }

    public void Init(IMoveSystem moveSystem)
    {
        _workSys = moveSystem;
    }

    public UnitGenerator Prepare(PropOwner owner, RaceType race)
    {
        _newUnit = GetNewUnit(race);
        _newUnit.LocalStatus.Owner = owner;
        _newUnit.UniversalStatus = DataManager.Instance.GetUnitData(race);
        _newUnit.SetMoveSystem(_workSys);
        return this;
    }

    private Unit GetNewUnit(RaceType race)
    {
        if (_inactives[race].TryDequeue(out var unit))
        {
            unit.gameObject.SetActive(true);
        }
        else
        {
            var unitHandler = Instantiate(_unitPrefab, _root);
            unit = AddUnitComponent(unitHandler, race);
            unit.ComponentHandler = unitHandler;
            _newUnit.gameObject.name = race.ToString();
        }

        return unit;
    }

    private Unit AddUnitComponent(UnitComponentHandler handler, RaceType race)
    {
        return race switch
        {
            RaceType.Human => handler.gameObject.AddComponent<Human>(),
            RaceType.Animal => handler.gameObject.AddComponent<Human>(),
            _ => throw new NotImplementedException()
        };
    }

    public UnitGenerator SetName(string name)
    {
        name ??= GetRandomName();
        _newUnit.LocalStatus.Name = name;
        return this;
    }

    private string GetRandomName()
    {
        return "랜덤";
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

    public Unit Generate()
    {
        var temp = _newUnit;
        Generated?.Invoke(_newUnit);
        return temp;
    }

    public void OnDestroyed(Unit obj)
    {
        Destroyed?.Invoke(_newUnit);
        _inactives[obj.UniversalStatus.Race].Enqueue(obj);
    }

    #region Register
    public void RegisterGenerated(Action<Unit> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<Unit> action)
    {
        Destroyed += action;
    }
    #endregion
}
