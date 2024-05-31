using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviourSingleton<BuildingGenerator>, IObjectGenerator<Building>
{
    private Building _prefab;

    private Queue<Building> _inactives;
    private Transform _root;
    private Building _newBuilding;

    private event Action<Building> Generated;
    private event Action<Building> Destroyed;

    private void Awake()
    {
        _prefab = Resources.Load<Building>("Prefabs/Building");
        _root = new GameObject("Buildings").transform;
        _inactives = new();

        var bluePrintMaterial = DataManager.Instance.BluePrintMaterial;
        Building.SetBluePrintMaterial(bluePrintMaterial);
    }

    public BuildingGenerator Prepare(BuildingType type)
    {
        _newBuilding = GetNewObject();
        SetBuildingStatus(type);
        return this;
    }

    private Building GetNewObject()
    {
        if (_inactives.TryDequeue(out var building))
        {
            building.gameObject.SetActive(true);
        }
        else
        {
            building = Instantiate(_prefab, _root);
            building.name = "Building";
        }
        return building;
    }

    private void SetBuildingStatus(BuildingType type)
    {
        var data = DataManager.Instance.GetBuildingData(type);
        _newBuilding.InitStatus(data);
    }

    public BuildingGenerator SetPosition(Vector2Int pos)
    {
        _newBuilding.transform.position = Util.Vector2IntToVector3(pos);
        return this;
    }

    public BuildingGenerator SetPosition(Vector3 pos)
    {
        _newBuilding.transform.position = Util.FloorVector3(pos);
        return this;
    }

    public BuildingGenerator ConvertBlueprint()
    {
        _newBuilding.ConvertBluePrint();
        return this;
    }

    public BuildingGenerator ConvertCompletion()
    {
        _newBuilding.ConvertCompletion();
        return this;
    }

    public Building Generate()
    {
        var building = _newBuilding;
        Generated?.Invoke(building);
        _newBuilding = null;
        return building;
    }

    public void OnDestroyed(Building building)
    {
        _inactives.Enqueue(building);
        Destroyed?.Invoke(building);
    }

    #region Register
    public void RegisterGenerated(Action<Building> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<Building> action)
    {
        Destroyed += action;
    }
    #endregion
}

public interface IObjectGenerator<T>
{
    public T Generate();
    public void OnDestroyed(T obj);
    public void RegisterGenerated(Action<T> action);
    public void RegisterDestroyed(Action<T> action);
}

public class ObjectPooling<T> where T : class
{
    private Queue<T> _inactives;

    public ObjectPooling(int count = 8)
    {
        _inactives = new(count);
    }
}

//public class ObjectPooling<T,Y> where T : Enum where Y : class
//{
//    private Dictionary<T,Queue<Y>> _inactives;

//    public ObjectPooling()
//    {
//        _inactives = Util.NewEnumKeyDictionary<T, Queue<Y>>();
//    }

//    public void Enqueue(T key, Y obj)
//    {
//        _inactives[key].Enqueue(obj);
//    }

//    public Y TryDequeue()
//    {


//    }
//}