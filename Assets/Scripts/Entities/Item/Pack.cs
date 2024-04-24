using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour, IPickupable, IAttackable, IItemStorage, ISelectable
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Collider _selectColider;
    [SerializeField] private GameObject _selectMarker;

    private IPackable _item;
    private bool _isSelection;
    private bool _isGenerateCarry; // TODO : Flag로 변경

    public static readonly List<InteractionType> DefaultInteraction
        = new() { InteractionType.Cancel, InteractionType.Carry };

    public IPackable Item { get { return _item; } }
    public Vector3 Position { get { return transform.position; } }
    public bool IsGenerateCarry { get { return _isGenerateCarry; } set { _isGenerateCarry = value; } }

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

    public void PickUp(IWorkable worker, int amount)
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
        if (_isGenerateCarry == true)
            return;

        WorkProcessHandler.Carry(this, amount);
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

    public SelectPropType GetSelectPropType()
    {
        return SelectPropType.Pack;
    }

    public void PutDownItem(IPackable item, Vector3 pos, int amount)
    {
        throw new System.NotImplementedException();
    }
}
