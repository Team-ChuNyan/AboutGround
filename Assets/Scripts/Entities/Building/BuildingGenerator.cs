using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviourSingleton<BuildingGenerator>
{
    private Building _prefab;

    private Queue<Building> _inactiveBuildings;
    private Transform _parentObject;
    private Building _newBuilding;

    private event Action<Building> Generated;
    private event Action<Building> Destroyed;

    private void Awake()
    {
        _prefab = Resources.Load<Building>("Prefabs/Building");
        _parentObject = new GameObject("Buildings").transform;
        _inactiveBuildings = new();

        var bluePrintMaterial = DataManager.Instance.BluePrintMaterial;
        Building.SetBluePrintMaterial(bluePrintMaterial);
    }

    public BuildingGenerator SetNewBuilding(BuildingType type)
    {
        _newBuilding = Instantiate(_prefab,_parentObject);
        _newBuilding.name = "Building";
        InitBuildingStatus(type);
        return this;
    }

    private void InitBuildingStatus(BuildingType type)
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

    public BuildingGenerator ChangeBlueprintMode()
    {
        _newBuilding.ConvertBluePrint();
        return this;
    }

    public Building GenerateBuilding()
    {
        var building = _newBuilding;
        Generated?.Invoke(building);
        _newBuilding = null;
        return building;
    }

    public void AfterDestoryCleanup(Building building)
    {
        _inactiveBuildings.Enqueue(building);
        Destroyed?.Invoke(building);
    }
}
