using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshRenderer _renderer;
    private Ground _gorund;

    public Ground Ground { get { return _gorund; } set { _gorund = value; } }

    public void SetMesh(Mesh mesh)
    {
        _filter.mesh = mesh;
    }

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }

}