using UnityEngine;

public class Pack : MonoBehaviour, IPickupable, IAttackable
{
    private Item _item;

    private bool _isFull;
    public bool IsFull { get { return _isFull;} }

    public void SetItem(Item newItem)
    {
        _item = newItem;
        _isFull = _item.Stack >= _item.ItemData.MaxStack;
    }

    public void StackItem(Pack target)
    {

    }


    public void PickUp()
    {

    }

    public void PutDown()
    {

    }
}
