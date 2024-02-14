using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    private GameObject _unitPrefab;
    private Dictionary<RaceType, Queue<Unit>> _inactiveUnit;
    private Unit _newUnit;

    private void Awake()
    {
        _unitPrefab = Resources.Load<GameObject>("Prefabs/Unit");
        _inactiveUnit = new();
        int enumCount = Enum.GetValues(typeof(RaceType)).Length;
        for (int i = 0; i < enumCount; i++)
        {
            _inactiveUnit.Add((RaceType)i, new());
        }
    }

    private void CreateNewUnit(RaceType type)
    {
        switch (type)
        {
            case RaceType.Human:
                var newobj = Instantiate(_unitPrefab);
                _newUnit = newobj.AddComponent<Human>();
                SetDefaultHumanUnitData();
                break;
            case RaceType.Animal:
                break;
            default:
                break;
        }
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

    public Unit GetNewUnit()
    {
        var temp = _newUnit;
        _newUnit = null;
        return temp;
    }

    private void SetDefaultHumanUnitData()
    {
        _newUnit.UnitData.SetDesc("asdddd");
        _newUnit.UnitData.SetMoveSpeed(1.0f);
    }

    private string GetRandomName()
    {
        return "랜덤";
    }
}
