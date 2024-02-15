using UnityEngine;

public class Pack : MonoBehaviour, IPickupable, IAttackable
{
    private IPackable _item;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public IPackable Item { get { return _item; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    public void SetItem(IPackable newItem)
    {
        _item = newItem;
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
