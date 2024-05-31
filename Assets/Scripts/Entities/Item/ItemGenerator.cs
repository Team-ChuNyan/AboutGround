using System;
using System.Collections.Generic;

public class ItemGenerator : Singleton<ItemGenerator>, IObjectGenerator<Item>
{
    private readonly Dictionary<ItemCategory, Queue<Item>> _inactives;
    private Item _newItem;

    private event Action<Item> Generated;
    private event Action<Item> Destroyed;

    public ItemGenerator()
    {
        _inactives = Util.NewEnumKeyDictionary<ItemCategory, Queue<Item>>();
    }

    public ItemGenerator Prepare(ItemType type)
    {
        _newItem = GetNewItem(type);
        SetUniversalStatus(type);
        return this;
    }

    public Item CopyItem(Item item)
    {
        Prepare(item.UniversalStatus.Type);
        SetStack(item.Amount);
        SetDurability(item.LocalStatus.Durability);
        return Generate();
    }

    public Item GetNewItem(ItemType type)
    {
        ItemCategory category = Util.SwitchItemTypeToItemCategory(type);
        if (_inactives[category].TryDequeue(out var item) == false)
        {
            if (category == ItemCategory.Equipment)
            {
                item = CreateEquipment(type);
            }
            else if (category == ItemCategory.Consumable)
            {
                item = CreateConsumable(type);
            }
            else if (category == ItemCategory.Resource)
            {
                item = CreateResource(type);
            }
        }

        return item;
    }

    private Item CreateConsumable(ItemType type)
    {
        var newConsumable = new Consumable();
        return newConsumable;
    }

    private Item CreateEquipment(ItemType type)
    {
        var newEquipment = new Equipment();
        return newEquipment;
    }

    private Item CreateResource(ItemType type)
    {
        var newResource = new Resource();
        return newResource;
    }

    private void SetUniversalStatus(ItemType type)
    {
        _newItem.UniversalStatus = DataManager.Instance.GetItemData(type);
    }

    public ItemGenerator SetStack(int value = int.MaxValue)
    {
        int amount = value < _newItem.UniversalStatus.MaxAmount ? value : _newItem.UniversalStatus.MaxAmount;
        _newItem.LocalStatus.Amount = amount;
        return this;
    }

    public ItemGenerator SetDurability(float value = int.MaxValue)
    {
        float amount = value < _newItem.UniversalStatus.MaxDurability ? value : _newItem.UniversalStatus.MaxDurability;
        _newItem.LocalStatus.Durability = amount;
        return this;
    }

    public Item Generate()
    {
        _newItem.LocalStatus.IsActivity = true;
        Generated?.Invoke(_newItem);
        return _newItem;
    }

    public void OnDestroyed(Item item)
    {
        ItemCategory category = Util.SwitchItemTypeToItemCategory(item.UniversalStatus.Type);
        _inactives[category].Enqueue(item);
        Destroyed?.Invoke(item);
    }

    #region Register
    public void RegisterGenerated(Action<Item> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<Item> action)
    {
        Destroyed += action;
    }
    #endregion
}
