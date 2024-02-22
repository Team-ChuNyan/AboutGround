using UnityEngine;

public abstract class Item : IPackable
{
    private ItemData _itemData;
    private int _amount;
    private float _durability;
    private bool _isActivity;

    public ItemData ItemData { get { return _itemData; } set { _itemData = value; } }
    public bool IsStack => ItemData.IsStacked;
    public int MaxAmount => _itemData.MaxAmount;
    public int Amount { get { return _amount; } set { _amount = value; } }
    public float Durability { get { return _durability; } set { _durability = value; } }
    public bool IsFull { get { return _amount >= ItemData.MaxAmount; } }
    public bool IsActivity { get { return _isActivity; } set { _isActivity = value; } }

    public Item SetNewItemData(ItemData data)
    {
        _itemData = data;
        _durability = data.MaxDurability;
        return this;
    }

    public Item SetAmount(int amount)
    {
        _amount = amount;
        return this;
    }

    public Item SetDurability(int value)
    {
        _durability = value < _itemData.MaxDurability ? value : _itemData.MaxDurability;
        return this;
    }

    public float GetDurabilityPercent()
    {
        return _durability / _itemData.MaxDurability * 100;
    }

    public void StackItem(IPackable packable)
    {
        if (packable is not Item item
         || _itemData.Type != item.ItemData.Type
         || _amount >= item.ItemData.MaxAmount)
            return;

        int allAmount = _amount + item.Amount;
        if (allAmount <= ItemData.MaxAmount)
        {
            _amount = allAmount;
            item.Amount = 0;
            return;
        }
        else
        {
            _amount = _itemData.MaxAmount;
            item.Amount = allAmount - _amount;
            return;
        }
    }

    public void Pack(Vector2Int respawn)
    {
        PackGenerator.Instance.CreateNewItemPack(this)
                              .SetPosition(respawn);
    }

    public IPackable CopyPack()
    {
        return ItemGenerator.Instance.CopyItem(this);
    }

    public void Deactivate()
    {
        _isActivity = false;
        ItemGenerator.Instance.DeactivateItem(this);
    }
}
