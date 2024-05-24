using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<RaceType, UnitData> _unitData;

    private Dictionary<ItemType, ItemUniversalStatus> _itemData;
    private Dictionary<ItemType, EquipmentStatus> _equipmentData;

    private Dictionary<BuildingType, BuildingUniversalStatus> _buildingGlobalStatus;
    public Material BluePrintMaterial { get; private set; }

    public DataManager() : base()
    {
        InitializeGameData();
    }

    private void InitializeGameData()
    {
        // TODO : DB에서 불러올 수 있도록
        LoadItemData();
        LoadEquipmentData();

        LoadUnitData();

        LoadBuildingData();
    }

    public ItemUniversalStatus GetItemData(ItemType type)
    {
        return _itemData[type];
    }

    public UnitData GetUnitData(RaceType type)
    {
        return _unitData[type];
    }

    public EquipmentStatus GetEquipmentData(ItemType type)
    {
        if (!_equipmentData.TryGetValue(type, out EquipmentStatus data))
        {
            Debug.LogError("Empty DataType");
        }

        return data;
    }

    public BuildingUniversalStatus GetBuildingData(BuildingType type)
    {
        _buildingGlobalStatus.TryGetValue(type, out var data);
        return data;
    }

    private void LoadItemData()
    {
        _itemData = new()
        {
            { ItemType.Shirt, new()},
            { ItemType.Wood, new()}
        };

        ItemUniversalStatus apple = new();
        apple.Type = ItemType.Apple;
        apple.MaxAmount = 99;
        apple.IsStacked = true;
        apple.Description = "Good Apple";
        apple.Weight = 0.1f;
        apple.Name = "Apple";
        apple.MaxDurability = 20;

        _itemData.Add(ItemType.Apple, apple);
    }

    private void LoadEquipmentData()
    {
        _equipmentData = new()
        {
            { ItemType.Shirt, new()},
        };
    }

    private void LoadUnitData()
    {
        _unitData = new();

        var unitData = new UnitData()
            .SetRace(RaceType.Human)
            .SetName("Dubug")
            .SetMoveSpeed(1)
            .SetDesc("Debug Test Unit");

        unitData.MaxWeight = 40;
        unitData.Weight = 0;

        _unitData.Add(RaceType.Human, unitData);
    }

    private void LoadBuildingData()
    {
        _buildingGlobalStatus = new();

        var rockGlobal = new BuildingUniversalStatus
        {
            BuildingType = BuildingType.Rock

        };

        var wallGlobal = new BuildingUniversalStatus
        {
            BuildingType = BuildingType.Wall
        };

        _buildingGlobalStatus.Add(BuildingType.Rock, rockGlobal);
        _buildingGlobalStatus.Add(BuildingType.Wall, wallGlobal);
        BluePrintMaterial = Resources.Load<Material>("Models/Building/Materials/BluePrint");
        // 
        foreach (var item in _buildingGlobalStatus ) 
        {
            var status = item.Value;
            status.Mesh = Resources.Load<Mesh>("Models/Building/Meshes/Test");
            status.Material = Resources.Load<Material>("Models/Building/Materials/Test");
        }

    }

}
