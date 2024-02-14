public abstract class Item
{
    private ItemData _itemData;
    private int _stack;
    private float _durability;

    public ItemData ItemData { get { return _itemData; } set { _itemData = value; } }
    public int Stack { get { return _stack; } set { _stack = value; } }
    public float Durability { get { return _durability; } set { _durability = value; } }

    public float GetDurabilityPercent()
    {
        return _durability / _itemData.MaxDurability * 100;
    }
}
