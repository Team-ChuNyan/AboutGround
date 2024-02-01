using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int Move_STARIGHT_COST = 10;
    private const int Move_DIAGONAL_COST = 14;

    private PathNode[,] _nodes;
    private NodePriorityQueue _openList;
    private HashSet<PathNode> _closeList;
    private Stack<PathNode> _stack;
    private PathNode _current;

    public Vector2Int Start;
    public Vector2Int Goal;

    public List<Vector2Int> DebugList;

    public void Awake()
    {
        DebugList.Clear();
        _nodes = new PathNode[200, 200];
        for (int i = 0; i < _nodes.GetLength(0); i++)
        {
            for (int k = 0; k < _nodes.GetLength(1); k++)
            {
                _nodes[i, k] = new PathNode(new Vector2Int(i, k));
            }
        }
        _stack = new();
        DebugList = new();
        _openList = new NodePriorityQueue(1500);
        _closeList = new HashSet<PathNode>();
    }

    public void FindPath(Vector2Int start, Vector2Int goal)
    {
        ResetPathNodeData();
        Goal = goal;
        _openList.Enqueue(_nodes[start.x, start.y]);

        while (_openList.Count > 0)
        {
            _current = _openList.Dequeue();
            if (_current.Pos == goal)
            {
                LinkPath(_current);
                GizmoTest();
                break;
            }
            AddAroundPathNode(_current);
            _closeList.Add(_current);
        }
    }

    private void ResetPathNodeData()
    {
        int lengthX = _nodes.GetLength(0);
        int lengthY = _nodes.GetLength(1);

        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 0; y < lengthY; y++)
            {
                _nodes[x, y].ResetPathfindingData();
            }
        }
    }

    private void AddAroundPathNode(PathNode target)
    {
        int centerX = target.Pos.x;
        int centerY = target.Pos.y;

        int leftX = centerX - 1;
        int rightX = centerX + 1;
        int bottomY = centerY - 1;
        int topY = centerY + 1;

        GetDiagonalNode(leftX, bottomY); // 1
        GetDiagonalNode(leftX, topY); // 7
        GetDiagonalNode(rightX, bottomY); // 3
        GetDiagonalNode(rightX, topY); // 9

        GetStraightNode(leftX, centerY);     // 4
        GetStraightNode(rightX, centerY);     // 6
        GetStraightNode(centerX, bottomY);     // 2
        GetStraightNode(centerX, topY);     // 8
    }

    private void GetStraightNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 200 || y >= 200) return;

        var node = _nodes[x, y];
        if (_closeList.Contains(node)) return;
        if (node.IsBlocked) return;
        if (node.IsClosed) return;

        if (_openList.Contains(node) == false)
        {
            node.BeforeNode = _current;
            node.G = Move_STARIGHT_COST + _current.G;
            node.H = GetHeuristic(node.Pos, Goal);
            node.F = node.G + node.H;
            _openList.Enqueue(node);
        }
        else if (Move_STARIGHT_COST + _current.G + node.H < node.F)
        {
            node.BeforeNode = _current;
            node.G = Move_STARIGHT_COST + _current.G;
            node.H = GetHeuristic(node.Pos, Goal);
            node.F = node.G + node.H;
        }
    }

    private void GetDiagonalNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 200 || y >= 200) return;

        var node = _nodes[x, y];
        if (_closeList.Contains(node)) return;
        if (node.IsBlocked) return;
        if (node.IsClosed) return;

        if (_openList.Contains(node) == false)
        {
            node.BeforeNode = _current;
            node.G = Move_DIAGONAL_COST + _current.G;
            node.H = GetHeuristic(node.Pos, Goal);
            node.F = node.G + node.H;
            _openList.Enqueue(node);
        }
        else if (Move_DIAGONAL_COST + _current.G + node.H <= node.F)
        {
            node.BeforeNode = _current;
            node.G = Move_DIAGONAL_COST + _current.G;
            node.H = GetHeuristic(node.Pos, Goal);
            node.F = node.G + node.H;
        }
    }

    private int GetHeuristic(Vector2Int start, Vector2Int end)
    {
        int disX = Mathf.Abs(start.x - end.x);
        int disY = Mathf.Abs(start.y - end.y);
        int remain = Mathf.Abs(disX - disY);

        return Move_DIAGONAL_COST * Mathf.Min(disX, disY) + Move_STARIGHT_COST * remain;
    }

    private void LinkPath(PathNode node)
    {
        _stack = new();
        while (node.BeforeNode != null)
        {
            node = node.BeforeNode;
            _stack.Push(node);
        }
    }

    private void GizmoTest()
    {
        while (_stack.Count > 0)
        {
            var asdf = _stack.Pop();
            DebugList.Add(asdf.Pos);
        }
    }

    private void OnDrawGizmos()
    {
        if (DebugList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < DebugList.Count - 1; i++)
        {
            Vector3 from = new Vector3(DebugList[i].x, DebugList[i].y);
            Vector3 to = new Vector3(DebugList[i + 1].x, DebugList[i + 1].y);
            Gizmos.DrawLine(from, to);
        }
    }
}