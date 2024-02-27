using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshRenderer _renderer;
    private PathNode _node;

    public PathNode Node { get { return _node; } set { _node = value; } }

    public void SetMesh(Mesh mesh)
    {
        _filter.mesh = mesh;
    }

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
    }

}