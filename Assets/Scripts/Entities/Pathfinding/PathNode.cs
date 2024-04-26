using UnityEngine;

public class PathNode : IPrioritizable
{
    public Vector2Int Pos;
    public bool IsBlocked;

    public int G;
    public int H;
    public int F;

    public PathNode BeforeNode;
    private int _queueIndex;

    public int Priority { get { return F; } }
    public int QueueIndex { get { return _queueIndex; } set { _queueIndex = value; } }

    public PathNode(Vector2Int pos)
    {
        Pos = pos;
        ResetPriorityData();
    }

    public void ResetPriorityData()
    {
        G = 0;
        H = 0;
        F = 0;
        BeforeNode = null;
        _queueIndex = 0;
    }
}
