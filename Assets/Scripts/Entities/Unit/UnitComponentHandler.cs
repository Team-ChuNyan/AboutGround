using UnityEngine;

public class UnitComponentHandler : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject _selectMaker;

    public Collider Collider { get { return _collider; } }
    public GameObject SelectMaker { get { return _selectMaker; } }
}
