using System.Collections.Generic;

public class ItemController
{
    private ItemFinding _itemFinding;
    private Dictionary<ItemType, List<Item>> _items;

    public ItemFinding ItemFinding { get { return _itemFinding; } }

    public ItemController()
    {
        _items = Util.NewEnumKeyDictionary<ItemType, List<Item>>();
        _itemFinding = new(_items);
    }

    public void Init()
    {
        ItemGenerator.Instance.RegisterGenerated(AddItem);
        ItemGenerator.Instance.RegisterDestroyed(RemoveItem);
    }

    private void AddItem(Item item)
    {
        ItemType type = item.UniversalStatus.Type;
        _items[type].Add(item);
    }

    private void RemoveItem(Item item)
    {
        ItemType type = item.UniversalStatus.Type;
        _items[type].Remove(item);
    }
}
