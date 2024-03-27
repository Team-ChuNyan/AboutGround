using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviourSingleton<UnitGenerator>
{
    private UnitComponentHandler _unitPrefab;
    private Dictionary<RaceType, Queue<Unit>> _inactiveUnit;
    private Unit _newUnit;

    private void Awake()
    {
        _unitPrefab = Resources.Load<UnitComponentHandler>("Prefabs/Unit");
        _inactiveUnit = Util.NewEnumKeyDictionary<RaceType, Queue<Unit>>();
    }

    private void CreateNewUnit(RaceType type)
    {
        switch (type)
        {
            case RaceType.Human:
                var newobj = Instantiate(_unitPrefab);
                _newUnit = newobj.gameObject.AddComponent<Human>();
                _newUnit.ComponentHandler = newobj;
                _newUnit.UnitData.SetRace(type);
                break;
            case RaceType.Animal:
                break;
            default:
                break;
        }

        SetDefaultUnitData();
        _newUnit.gameObject.name = "Unit";
    }

    public UnitGenerator SetNewUnit(RaceType type)
    {
        CreateNewUnit(type);
        return this;
    }

    public UnitGenerator SetName(string name)
    {
        name ??= GetRandomName();
        _newUnit.UnitData.SetName(name);
        return this;
    }

    public UnitGenerator SetMoveSystem(IMoveSystem sys)
    {
        _newUnit.SetMoveSystem(sys);
        return this;
    }

    public UnitGenerator SetPosition(Vector3 pos)
    {
        _newUnit.transform.position = pos;
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
    }

    private string GetRandomName()
    {
        return "랜덤";
    }
}
