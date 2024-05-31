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
    public bool IsSelection { get { return _isSelection; } }
    public bool IsGenerateCarry { get { return _isGenerateCarry; } set { _isGenerateCarry = value; } }

    public void OnDisable()
    {
        _isGenerateCarry = false;
        if (_isSelection == true)
        {
            CancelSelection();
        }
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public void SetMesh(Mesh mesh, Material material)
    {
        _meshFilter.mesh = mesh;
        _renderer.material = material;
    }

    public void SetItem(IPackable newItem)
    {
        _item = newItem;
        _item.OnPublicAccess(Position);
    }

    public void KeepItem(IPackable item, int amount)
    {
        _item.StackItem(item);
    }

    public void PickUp(IWorkable worker, int amount)
    {
        var inven = worker.GetInventory();

        inven.KeepItem(_item, amount);
        if (_item.Amount <= 0)
        {
            PackGenerator.Instance.OnDestroyed(this);
        }
    }

    public void CreateCarryWork(int amount = int.MaxValue)
    {
        if (_isGenerateCarry == true)
            return;

        WorkProcessHandler.Carry(this, amount);
    }

    public void Destroy()
    {
        // TODO : 아이템이 옮겨 가는지 완전히 파괴되는지 체크는 어떻게 할 것 인가?
        _item.OffPublicAccess();
        _item = null;
        PackGenerator.Instance.OnDestroyed(this);
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

    public void FinishWork(WorkType type)
    {
        _isGenerateCarry = false;
    }
}
