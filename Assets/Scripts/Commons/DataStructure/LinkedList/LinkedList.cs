using System;
using System.Collections;
using System.Collections.Generic;

namespace AboutGround.Commons.DataStructure
{
    public class LinkedList<T> : IEnumerable<LinkedListNode<T>>, IEnumerator<LinkedListNode<T>>
    {
        private readonly Queue<LinkedListNode<T>> _inactiveNodes;
        private readonly LinkedListNode<T> _head;
        private readonly LinkedListNode<T> _tail;
        private int count = 0;

        public int Count { get { return count; } }

        public LinkedList()
        {
            _inactiveNodes = new(32);
            _head = new();
            _tail = new();
            _head.After = _tail;
            _tail.Before = _head;
        }

        private LinkedListNode<T> GenerateNewNode()
        {
            if (_inactiveNodes.TryDequeue(out var newNode) == true)
            {
                newNode.Clear();
            }
            else
            {
                newNode = new LinkedListNode<T>();
            }

            return newNode;
        }

        public LinkedListNode<T> AddLast(T item)
        {
            var newNode = GenerateNewNode();
            newNode.Item = item;
            var before = _tail.Before;
            var after = _tail;
            Insert(before, after, newNode);
            count++;
            return newNode;
        }

        public LinkedListNode<T> AddFirst(T item)
        {
            var newNode = GenerateNewNode();
            newNode.Item = item;
            var after = _head.After;
            var before = _head;
            Insert(before, after, newNode);
            count++;
            return newNode;
        }

        public void Remove(LinkedListNode<T> node)
        {
            node.Remove();
            count--;
            _inactiveNodes.Enqueue(node);
        }

        private static void Insert(LinkedListNode<T> before, LinkedListNode<T> after, LinkedListNode<T> item)
        {
            item.Before = before;
            item.After = after;

            before.After = item;
            after.Before = item;
        }

        #region IEnumerable
        private LinkedListNode<T> _current;

        public LinkedListNode<T> Current { get { return _current; } }
        object IEnumerator.Current { get { return Current; } }

        public IEnumerator<LinkedListNode<T>> GetEnumerator()
        {
            var item = _head;
            for (int i = 0; i < count; i++)
            {
                item = item.After;
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            var item = _current.After;
            if (item is null)
                return false;

            _current = item;
            return true;
        }

        public void Reset()
        {
            _current = _head;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}