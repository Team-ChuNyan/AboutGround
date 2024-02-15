public class ItemGenerator : Generator<ItemCategory, Item>
{
    public ItemGenerator SetNewItem(ItemType type, int stack, int durability)
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
        SetCommonItemData(stack, durability);

        return this;
    }

    private void CreateConsumable(ItemType type)
    {
        var newConsumable = NewItemClass<Consumable>(type);
    }

    private void CreateEquipment(ItemType type)
    {
        var newEquipment = NewItemClass<Equipment>(type);
        var data = DataManager.Instance.GetEquipmentData(type);
        newEquipment.SetEquipmentData(data);
    }

    private void CreateResource(ItemType type)
    {
        var newResource = NewItemClass<Resource>(type);
    }

    private T NewItemClass<T>(ItemType type) where T : Item, new()
    {
        T item = new();
        _newItem = item;
        var data = DataManager.Instance.GetItemData(type);
        item.SetNewItemData(data);
        return item;
    }

    private void SetCommonItemData(int stack, int durability)
    {
        _newItem.Stack = stack;
        _newItem.Durability = durability;
    }

    public Item GetNewItem()
    {
        return _newItem;
    }
}
