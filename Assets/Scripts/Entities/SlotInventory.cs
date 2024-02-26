using System.Collections.Generic;

public class SlotInventory : IItemStorage
{
    private List<IPackable> _list;
    private int _maxSlot;
    private int _slot;

    public SlotInventory(int maxItemSlot)
    {
        _list = new List<IPackable>(maxItemSlot);
        SetMaxSlot(maxItemSlot);
    }

    public void SetMaxSlot(int maxItemSlot)
    {
        _maxSlot = maxItemSlot;
    }

    public void KeepItem(IPackable item)
    {
        OverlapInventoryItem(item);
        AddItemInSpareSpace(item);
    }

    private void OverlapInventoryItem(IPackable item)
    {
        if (item.IsStack == false)
            return;

        foreach (IPackable bagItem in _list)
        {
            bagItem.StackItem(item);
            if (item.Amount == 0)
                return;
        }

        return;
    }

    private void AddItemInSpareSpace(IPackable item)
    {
        while (_slot < _maxSlot)
        {
            var newItem = item.CopyPack();
            _list.Add(newItem);
            _slot++;
            if (item.Amount - item.MaxAmount > 0)
            {
                item.Amount -= item.MaxAmount;
                newItem.Amount = item.MaxAmount;
            }
            else
            {
                item.Amount = 0;
                return;
            }
        }
    }
}