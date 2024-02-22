using System.Collections.Generic;

public class ItemGenerator : Singleton<ItemGenerator>
{
    private Dictionary<ItemCategory, List<Item>> _inactiveItem;
    private Item _newItem;

    public ItemGenerator()
    {
        _inactiveItem = Util.NewEnumKeyDictionary<ItemCategory, List<Item>>();
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
        _newItem.IsActivity = true;
        return this;
    }

    public Item CopyItem(Item item)
    {
        SetNewItem(item.ItemData.Type);
        SetPersonalData(item.Amount, item.Durability);
        return GetNewItem();
    }

    private void CreateConsumable(ItemType type)
    {
        var newConsumable = new Consumable();
        _newItem = newConsumable;
    }

    private void CreateEquipment(ItemType type)
    {
        var newEquipment = new Equipment();
        var data = DataManager.Instance.GetEquipmentData(type);
        newEquipment.SetEquipmentData(data);

        _newItem = newEquipment;
    }

    private void CreateResource(ItemType type)
    {
        var newResource = new Resource();
        _newItem = newResource;
    }

    private void SetItemData(ItemType type)
    {
        _newItem.ItemData = DataManager.Instance.GetItemData(type);
    }

    public ItemGenerator SetPersonalData(int stack, float currentDurability = float.MaxValue)
    {
        _newItem.Amount = stack;
        _newItem.Durability = currentDurability == int.MaxValue ? _newItem.ItemData.MaxAmount : currentDurability;

        return this;
    }

    public Item GetNewItem()
    {
        return _newItem;
    }

    public void DeactivateItem(Item item)
    {
        ItemCategory category = Util.SwitchItemTypeToItemCategory(item.ItemData.Type);
        _inactiveItem[category].Add(item);
    }
}
