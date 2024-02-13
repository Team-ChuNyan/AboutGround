using System;

public class NodePriorityQueue // 최소값 이진 트리
{
    private readonly PathNode[] _nodes;
    private int _count;

    public int Count { get { return _count; } }

    public NodePriorityQueue(int Capacity)
    {
        _count = 0;
        _nodes = new PathNode[Capacity];
    }

    public void Enqueue(PathNode node)
    {
        _count++;
        _nodes[_count] = node;
        node.QueueIndex = _count;
        CompareParent(node);
    }

    public PathNode Dequeue()
    {
        var firstNode = _nodes[1];
        if (_count > 1)
        {
            var lastNode = _nodes[_count];
            _nodes[1] = lastNode;
            _nodes[1].QueueIndex = 1;
            CompareChildren(_nodes[1]);
        }
        _count--;
        return firstNode;
    }

    public void Clear()
    {
        for (int i = 1; i < _count; i++)
        {
            _nodes[i].ResetPathfindingData();
        }
        Array.Clear(_nodes, 1, _count);
        _count = 0;
    }

    public bool Contains(PathNode node)
    {
        return _nodes[node.QueueIndex] == node;
    }

    private void CompareParent(PathNode node)
    {
        while (node.QueueIndex > 1)
        {
            int parentIndex = node.QueueIndex >> 1;
            PathNode parentNode = _nodes[parentIndex];
            if (node.F >= parentNode.F)
                break;

            SwapIndex(node, parentNode);
        }
    }

    private void CompareChildren(PathNode node)
    {
        int leftIndex;
        int rightIndex;
        int childIndex;

        while (true)
        {
            leftIndex = (node.QueueIndex << 1);

            if (leftIndex > _count) break;
            if (leftIndex == _count)
                childIndex = leftIndex;
            else
            {
                rightIndex = leftIndex + 1;
                childIndex = _nodes[leftIndex].F < _nodes[rightIndex].F ? leftIndex : rightIndex;
            }

            if (_nodes[childIndex].F >= node.F)
                break;

            SwapIndex(node, _nodes[childIndex]);
        }
    }

    private void SwapIndex(PathNode A, PathNode B)
    {
        int temp = A.QueueIndex;
        A.QueueIndex = B.QueueIndex;
        B.QueueIndex = temp;

        _nodes[A.QueueIndex] = A;
        _nodes[B.QueueIndex] = B;
    }
}
