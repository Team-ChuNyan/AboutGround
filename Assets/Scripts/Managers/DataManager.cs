using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<RaceType, UnitData> _unitData;

    private Dictionary<ItemType, ItemData> _itemData;
    private Dictionary<ItemType, EquipmentData> _equipmentData;

    public ItemData GetItemData(ItemType type)
    {
        return _itemData[type];
    }

    public UnitData GetUnitData(RaceType type)
    {
        return _unitData[type];
    }

    public EquipmentData GetEquipmentData(ItemType type)
    {
        if (!_equipmentData.TryGetValue(type, out EquipmentData data))
        {
            Debug.LogError("Empty DataType");
        }

        return data;
    }

    public DataManager InitializeItemData()
    {
        // TODO : DB에서 불러올 수 있도록
        LoadItemData();
        LoadEquipmentData();

        LoadUnitData();

        return this;
    }

    private void LoadItemData()
    {
        _itemData = new()
        {
            { ItemType.Shirt, new()},
            { ItemType.Wood, new()}
        };

        ItemData apple = new();
        apple.MaxStack = 99;
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
}
