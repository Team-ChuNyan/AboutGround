using System;
using UnityEngine;

public class Pack : MonoBehaviour, IPickupable, IAttackable, IItemStorage
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private IPackable _item;

    public IPackable Item { get { return _item; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    public void SetItem(IPackable newItem)
    {
        _item = newItem;
    }

    public void KeepItem(IPackable item)
    {
        _item.StackItem(item);
    }

    public void PickUp(IWorkable worker)
    {
        var inven = worker.GetInventory();

        inven.KeepItem(_item);
        if (_item.Amount <= 0)
        {
            PackGenerator.Instance.DestoryPack(this);
        }
    }

    public void CreateCarryWork(int amount = int.MaxValue)
    {
        Vector2Int workPos = Util.Vector3ToVector2Int(transform.position);
        Work work = WorkGenerator.Instance.CreateNewWork(WorkType.Carry, amount)
                    .SetWorkPos(workPos)
                    .AddWorkPlan()
                    .GetWork();

        Action action = () => { PickUp(work.AssignWorker); };
        work.RegisterOnStarted(action);
    }

    public void DestroyPack()
    {
        _item = null;
        PackGenerator.Instance.DestoryPack(this);
    }
}
