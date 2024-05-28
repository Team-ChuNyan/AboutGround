using UnityEngine;

public class Building : MonoBehaviour, IItemStorage, IAttackable
{
    private static Material _bluePrint;

    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshRenderer _renderer;

    private BuildingUniversalStatus _globalStatus;
    private BuildingLocalStatus _localStatus;

    public BuildingUniversalStatus GlobalStatus { get { return _globalStatus; } }
    public BuildingLocalStatus LocalStatus { get { return _localStatus; } }

    private void Awake()
    {
        _localStatus = new();
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public static void SetBluePrintMaterial(Material material)
    {
        _bluePrint = material;
    }

    public void InitStatus(BuildingUniversalStatus globalStatus)
    {
        _globalStatus = globalStatus;
        _localStatus.Init(globalStatus);
        _filter.mesh = globalStatus.Mesh;
        _renderer.material = globalStatus.Material;
    }

    public void ConvertBluePrint()
    {
        if (_localStatus.IsBluePrint == true)
            return;

        _renderer.material = _bluePrint;
        _localStatus.IsBluePrint = true;
    }

    public void ConvertCompletion()
    {
        if (_localStatus.IsBluePrint == false)
            return;

        _renderer.material = _globalStatus.Material;
        _localStatus.IsBluePrint = false;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        BuildingGenerator.Instance.AfterDestoryCleanup(this);
    }

    public void KeepItem(IPackable item, int amount)
    {
        _localStatus.Inventory.KeepItem(item, amount);
    }

    public void PutDownItem(IPackable item, Vector3 pos, int amount)
    {
        throw new System.NotImplementedException();
    }
}
