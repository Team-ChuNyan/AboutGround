using System;
using UnityEngine;

public class Pack : MonoBehaviour, IPickupable, IAttackable, IItemStorage, ISelectable
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Collider _selectColider;
    [SerializeField] private GameObject _selectMarker;

    private IPackable _item;
    private bool _isSelection;

    public IPackable Item { get { return _item; } }

    public void SetMesh(Mesh mesh, Material material)
    {
        _meshFilter.mesh = mesh;
        _renderer.material = material;
    }

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

    public void AddSelection()
    {
        if (_isSelection == false)
        {
            _selectMarker.SetActive(true);
            _isSelection = true;
        }
        _selectMarker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void WaitSelection()
    {
        if (_isSelection == false)
        {
            _selectMarker.SetActive(true);
            _isSelection = true;
        }
        _selectMarker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public void CancelSelection()
    {
        _selectMarker.SetActive(false);
        _isSelection = false;
    }

    public Vector3 GetSelectPoint()
    {
        return _selectColider.bounds.center;
    }
}
