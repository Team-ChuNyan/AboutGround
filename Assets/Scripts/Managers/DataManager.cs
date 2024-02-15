using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<ItemType, ItemData> _itemData;
    private Dictionary<ItemType, EquipmentData> _equipmentData;

    public ItemData GetItemData(ItemType type)
    {
        return _itemData[type];
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

        return this;
    }

    private void LoadItemData()
    {
        _itemData = new()
        {
            { ItemType.Shirt, new()},
            { ItemType.Apple, new()},
            { ItemType.Wood, new()}
        };
    }

    private void LoadEquipmentData()
    {
        _equipmentData = new()
        {
            { ItemType.Shirt, new()},
        };
    }
}
