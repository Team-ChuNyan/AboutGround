using System.Collections.Generic;
using UnityEngine;

public class GroundPathfinding : IMoveSystem
{
    private const int Move_STARIGHT_COST = 10;
    private const int Move_DIAGONAL_COST = 14;
    private const int _weight = 1;

    private Ground[,] _nodes;
    private readonly PriorityQueue<Ground> _openList;
    private readonly HashSet<Ground> _closeList;
    private Ground _current;

    private Vector2Int _nodeMapSize;
    private Vector2Int _goal;

    public GroundPathfinding()
    {
        _openList = new PriorityQueue<Ground>(1024);
        _closeList = new HashSet<Ground>(16384);
    }

    public void SetNodeMap(Ground[,] nodes)
    {
        _nodes = nodes;
        _nodeMapSize = new(_nodes.GetLength(0), _nodes.GetLength(1));
    }

    public void UpdateMovementPath(List<Ground> path, Vector2Int start, Vector2Int goal)
    {
        if (_nodes[goal.x, goal.y].LocalStatus.IsBlocked
         || _nodes[start.x, start.y].LocalStatus.IsBlocked)
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
            if (_current.LocalStatus.Pos != goal)
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
            node.ResetPriorityData();
        }
        _closeList.Clear();
        _openList.Clear();
    }

    private void AddAroundPathNode(Ground target)
    {
        int centerX = target.LocalStatus.Pos.x;
        int centerY = target.LocalStatus.Pos.y;

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
        if (_closeList.Contains(node) || node.LocalStatus.IsBlocked) return;
        if (moveCost == Move_DIAGONAL_COST)
        {
            var currentPos = _current.LocalStatus.Pos;
            var conorPos = node.LocalStatus.Pos - currentPos;

            if (_nodes[currentPos.x + conorPos.x, currentPos.y].LocalStatus.IsBlocked
             || _nodes[currentPos.x, currentPos.y + conorPos.y].LocalStatus.IsBlocked)
            {
                return;
            }
        }

        if (_openList.Contains(node) == false)
        {
            AddNewNodeToOpenList(node, moveCost);
        }
        else if (moveCost + _current.PathFindingData.G + node.PathFindingData.H < node.PathFindingData.F)
        {
            RefreshNode(node, moveCost);
        }
    }

    private void AddNewNodeToOpenList(Ground node, int moveCost)
    {
        node.PathFindingData.H = CalculateHeuristic(node.LocalStatus.Pos, _goal);
        RefreshNode(node, moveCost);
        _openList.Enqueue(node);
    }

    private void RefreshNode(Ground node, int moveCost)
    {
        node.PathFindingData.BeforeNode = _current;
        var data = node.PathFindingData;
        int g = moveCost + _current.PathFindingData.G;
        int h = data.H;
        int f = data.G + h;
        AStarPathFindingData newData = new(_current, data.QueueIndex, g, h, f);

        node.PathFindingData = newData;
    }

    private int CalculateHeuristic(Vector2Int start, Vector2Int end)
    {
        int disX = Mathf.Abs(start.x - end.x);
        int disY = Mathf.Abs(start.y - end.y);
        return (disX + disY) << _weight;
    }

    private void ConnectMovementPath(Ground node, List<Ground> path)
    {
        path.Clear();
        path.Add(node);
        while (node.PathFindingData.BeforeNode != null)
        {
            node = node.PathFindingData.BeforeNode;
            path.Add(node);
        }
        path.RemoveAt(path.Count - 1);
        path.Reverse();
    }
}
