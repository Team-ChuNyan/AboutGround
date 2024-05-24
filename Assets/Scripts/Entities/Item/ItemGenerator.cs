using System;
using System.Collections.Generic;

public class ItemGenerator : Singleton<ItemGenerator>
{
    private Dictionary<ItemCategory, Queue<Item>> _inactiveItem;
    private Item _newItem;

    private event Action<Item> Generated;
    private event Action<Item> Destroyed;

    public ItemGenerator()
    {
        _inactiveItem = Util.NewEnumKeyDictionary<ItemCategory, Queue<Item>>();
    }

    public ItemGenerator SetNewItem(ItemType type)
    {
        ItemCategory category = Util.SwitchItemTypeToItemCategory(type);

        switch (category)
        {
            case ItemCategory.Equipment:
                CreateEquipment(type);
                break;
            case ItemCategory.Consumable:
                CreateConsumable(type);
                break;
            case ItemCategory.Resource:
                CreateResource(type);
                break;
        }

        SetItemData(type);
        return this;
    }

    public Item CopyItem(Item item)
    {
        SetNewItem(item.UniversalStatus.Type);
        SetLocalData(item.Amount, item.LocalStatus.Durability);
        return Generate();
    }

    private void CreateConsumable(ItemType type)
    {
        var newConsumable = new Consumable();
        _newItem = newConsumable;
    }

    private void CreateEquipment(ItemType type)
    {
        if (_inactiveItem[ItemCategory.Equipment].TryDequeue(out var equipment) == false)
        {
            equipment = new Equipment();
        }

        var data = DataManager.Instance.GetEquipmentData(type);
        ((Equipment)equipment).SetEquipmentData(data);

        _newItem = equipment;
    }

    private void CreateResource(ItemType type)
    {
        var newResource = new Resource();
        _newItem = newResource;
    }

    private void SetItemData(ItemType type)
    {
        _newItem.UniversalStatus = DataManager.Instance.GetItemData(type);
    }

    public ItemGenerator SetLocalData(int stack, float currentDurability = float.MaxValue)
    {
        _newItem.Amount = stack;
        _newItem.LocalStatus.Durability = currentDurability == int.MaxValue ? _newItem.UniversalStatus.MaxAmount : currentDurability;

        return this;
    }

    public Item Generate()
    {
        _newItem.LocalStatus.IsActivity = true;
        OnGenerated(_newItem);
        return _newItem;
    }

    public void Remove(Item item)
    {
        ItemCategory category = Util.SwitchItemTypeToItemCategory(item.UniversalStatus.Type);
        _inactiveItem[category].Enqueue(item);
        OnDestroyed(item);
    }

    public void RegisterGenerated(Action<Item> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<Item> action)
    {
        Destroyed += action;
    }

    private void OnGenerated(Item item)
    {
        Generated?.Invoke(item);
    }

    private void OnDestroyed(Item item)
    {
        Generated?.Invoke(item);
    }
}
