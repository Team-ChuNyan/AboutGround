using UnityEngine;

public abstract class Item : IPackable
{
    public ItemUniversalStatus UniversalStatus;
    public ItemLocalStatus LocalStatus;

    public bool IsStack => UniversalStatus.IsStacked;
    public int MaxAmount => UniversalStatus.MaxAmount;
    public int Amount { get { return LocalStatus.Amount; } set { LocalStatus.Amount = value; } }
    public bool IsFull { get { return LocalStatus.Amount >= UniversalStatus.MaxAmount; } }

    public Item()
    {
        LocalStatus = new();
    }

    public Item SetNewItemData(ItemUniversalStatus data)
    {
        UniversalStatus = data;
        LocalStatus.Durability = data.MaxDurability;
        return this;
    }

    public Item SetAmount(int amount)
    {
        LocalStatus.Amount = amount;
        return this;
    }

    public Item SetDurability(int value)
    {
        LocalStatus.Durability = value < UniversalStatus.MaxDurability ? value : UniversalStatus.MaxDurability;
        return this;
    }

    public float GetDurabilityPercent()
    {
        return LocalStatus.Durability / UniversalStatus.MaxDurability * 100;
    }

    public void StackItem(IPackable packable)
    {
        if (packable is not Item item
         || UniversalStatus.Type != item.UniversalStatus.Type
         || LocalStatus.Amount >= item.UniversalStatus.MaxAmount)
            return;

        int allAmount = LocalStatus.Amount + item.Amount;
        if (allAmount <= UniversalStatus.MaxAmount)
        {
            LocalStatus.Amount = allAmount;
            item.Amount = 0;
            return;
        }
        else
        {
            LocalStatus.Amount = UniversalStatus.MaxAmount;
            item.Amount = allAmount - LocalStatus.Amount;
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

    public void Destroy()
    {
        LocalStatus.Init();
        ItemGenerator.Instance.Remove(this);
    }
}
