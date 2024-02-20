using System.Collections.Generic;
using UnityEngine;

public class GroundPathfinding : IMoveSystem
{
    private const int Move_STARIGHT_COST = 10;
    private const int Move_DIAGONAL_COST = 14;
    private const int _weight = 1;

    private PathNode[,] _nodes;
    private readonly NodePriorityQueue _openList;
    private readonly HashSet<PathNode> _closeList;
    private PathNode _current;

    private Vector2Int _nodeMapSize;
    private Vector2Int _goal;

    public GroundPathfinding()
    {
        _openList = new NodePriorityQueue(1024);
        _closeList = new HashSet<PathNode>(16384);
    }

    public void SetNodeMap(PathNode[,] nodes)
    {
        _nodes = nodes;
        _nodeMapSize = new(_nodes.GetLength(0), _nodes.GetLength(1));
    }

    public void UpdateMovementPath(List<PathNode> path, Vector2Int start, Vector2Int goal)
    {
        if (_nodes[goal.x, goal.y].IsBlocked
         || _nodes[start.x, start.y].IsBlocked)
            return;

        FindPath(start, goal);
        ConnectMovementPath(_current, path);
        ResetPathNodeData();
    }

    private void FindPath(Vector2Int start, Vector2Int goal)
    {
        _goal = goal;
        _openList.Enqueue(_nodes[start.x, start.y]);

        while (_openList.Count > 0)
        {
            _current = _openList.Dequeue();
            _closeList.Add(_current);
            if (_current.Pos != goal)
            {
                AddAroundPathNode(_current);
            }
            else
            {
                break;
            }
        }
    }

    private void ResetPathNodeData()
    {
        foreach (var node in _closeList)
        {
            node.ResetPathfindingData();
        }
        _closeList.Clear();
        _openList.Clear();
    }

    private void AddAroundPathNode(PathNode target)
    {
        int centerX = target.Pos.x;
        int centerY = target.Pos.y;

        int leftX = centerX - 1;
        int rightX = centerX + 1;
        int bottomY = centerY - 1;
        int topY = centerY + 1;

        TryAddPathNode(leftX, centerY, Move_STARIGHT_COST);        // 4
        TryAddPathNode(rightX, centerY, Move_STARIGHT_COST);       // 6
        TryAddPathNode(centerX, bottomY, Move_STARIGHT_COST);      // 2
        TryAddPathNode(centerX, topY, Move_STARIGHT_COST);         // 8

        TryAddPathNode(leftX, bottomY, Move_DIAGONAL_COST);        // 1
        TryAddPathNode(leftX, topY, Move_DIAGONAL_COST);           // 7 
        TryAddPathNode(rightX, bottomY, Move_DIAGONAL_COST);       // 3
        TryAddPathNode(rightX, topY, Move_DIAGONAL_COST);          // 9
    }

    private void TryAddPathNode(int x, int y, int moveCost)
    {
        if ((x < 0 || y < 0 || x >= _nodeMapSize.x || y >= _nodeMapSize.y)) return;

        var node = _nodes[x, y];
        if (_closeList.Contains(node) || node.IsBlocked) return;
        if (moveCost == Move_DIAGONAL_COST)
        {
            var currentPos = _current.Pos;
            var conorPos = node.Pos - currentPos;

            if (_nodes[currentPos.x + conorPos.x, currentPos.y].IsBlocked
             || _nodes[currentPos.x, currentPos.y + conorPos.y].IsBlocked)
            {
                return;
            }
        }

        if (_openList.Contains(node) == false)
        {
            AddNewNodeToOpenList(node, moveCost);
        }
        else if (moveCost + _current.G + node.H < node.F)
        {
            RefreshNode(node, moveCost);
        }
    }

    private void AddNewNodeToOpenList(PathNode node, int moveCost)
    {
        node.H = CalculateHeuristic(node.Pos, _goal);
        RefreshNode(node, moveCost);
        _openList.Enqueue(node);
    }

    private void RefreshNode(PathNode node, int moveCost)
    {
        node.BeforeNode = _current;
        node.G = moveCost + _current.G;
        node.F = node.G + node.H;
    }

    private int CalculateHeuristic(Vector2Int start, Vector2Int end)
    {
        int disX = Mathf.Abs(start.x - end.x);
        int disY = Mathf.Abs(start.y - end.y);
        return (disX + disY) << _weight;
    }

    private void ConnectMovementPath(PathNode node, List<PathNode> path)
    {
        path.Clear();
        path.Add(node);
        while (node.BeforeNode != null)
        {
            node = node.BeforeNode;
            path.Add(node);
        }
        path.RemoveAt(path.Count - 1);
        path.Reverse();
    }
}
