public static class Util
{
    public static ItemCategory SwitchItemTypeToItemCategory(ItemType itemType)
    {
        return (int)itemType switch
        {
            >= (int)ItemCategory.Resource => ItemCategory.Resource,
            >= (int)ItemCategory.Consumable => ItemCategory.Consumable,
            >= (int)ItemCategory.Equipment => ItemCategory.Equipment,
            _ => ItemCategory.Resource,
        };
    }
}
