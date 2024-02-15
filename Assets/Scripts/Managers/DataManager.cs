using System.Collections.Generic;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<ItemType, Item> _itemData;

    public Item GetItemData(ItemType type)
    {
        return _itemData[type];
    }

    public DataManager InitializeItemData()
    {
        // TODO : DB에서 불러올 수 있도록
        _itemData = new Dictionary<ItemType, Item>
        {
            { ItemType.Shirt, new Equipment()},
            { ItemType.Apple, new Consumable()},
            { ItemType.Wood, new Resource()}
        };

        return this;
    }
}
