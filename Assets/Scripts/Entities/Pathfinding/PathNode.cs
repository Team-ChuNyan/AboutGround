using UnityEngine;

public class PathNode
{
    public Vector2Int Pos;
    public bool IsClosed;
    public bool IsBlocked;

    public int G;
    public int H;
    public int F;

    public PathNode BeforeNode;
    public int QueueIndex;

    public PathNode(Vector2Int pos)
    {
        Pos = pos;
        ResetPathfindingData();
    }

    public void ResetPathfindingData()
    {
        G = 0;
        H = 0;
        F = 0;
        IsClosed = false;
        BeforeNode = null;
        QueueIndex = 0;
    }
}
