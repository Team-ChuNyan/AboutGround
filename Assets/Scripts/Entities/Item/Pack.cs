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

    public void PickUp(IWorkable worker)
    {

    }

    public void PutDown(IWorkable worker)
    {

    }

    public Work CreateCarryWork(int amount)
    {
        Work work = new Work(); // TODO : 오브젝트 풀링
        work.SetWorkData(WorkType.Carry, amount)
            .SetPriority();

        return work;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Work CreateCarryWork()
    {
        throw new System.NotImplementedException();
    }
}
