using UnityEngine;

public class Work
{
    private Vector2Int _pos; 
    private WorkType _type;
    private int _workload;

    public Vector2Int Pos { get { return _pos; } set { _pos = value; } }

    public Work(Vector2Int pos, WorkType type, int workload)
    {
        Pos = pos;
        _type = type;
        _workload = workload;
    }
}
