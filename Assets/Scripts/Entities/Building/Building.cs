using UnityEngine;

public class Building : MonoBehaviour
{
    private static Material _bluePrint;

    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshRenderer _renderer;

    private BuildingUniversalStatus _globalStatus;
    private BuildingLocalStatus _status;

    public BuildingUniversalStatus GlobalStatus { get { return _globalStatus; } }
    public BuildingLocalStatus Status { get { return _status; } }

    public static void SetBluePrintMaterial(Material material)
    {
        _bluePrint = material;
    }

    public void InitStatus(BuildingUniversalStatus globalStatus)
    {
        _globalStatus = globalStatus;
        _status = new(globalStatus);
        _filter.mesh = globalStatus.Mesh;
        _renderer.material = globalStatus.Material;
    }

    public void ConvertBluePrint()
    {
        if (_status.IsBluePrint == true)
            return;

        _renderer.material = _bluePrint;
        _status.IsBluePrint = true;
    }

    public void ConvertCompletion()
    {
        if (_status.IsBluePrint == false)
            return;

        _renderer.material = _globalStatus.Material;
        _status.IsBluePrint = false;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        BuildingGenerator.Instance.AfterDestoryCleanup(this);
    }
}
