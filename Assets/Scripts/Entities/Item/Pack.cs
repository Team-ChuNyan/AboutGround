using System;
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

    public void PickUp(int count)
    {

    }

    public void PutDown(int count)
    {

    }

    public void CreateCarryWork(int amount = int.MaxValue)
    {
        Vector2Int workPos = Util.Vector3ToVector2Int(transform.position);
        Work work = WorkGenerator.Instance.CreateNewWork(WorkType.Carry, amount)
                    .SetWorkPos(workPos)
                    .GetWork();

        Action action = () => { PickUp(amount); };
        work.RegisterOnStarted(action);
    }
}
