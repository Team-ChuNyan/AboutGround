namespace AboutGround.Commons.DataStructure
{
    public class LinkedListNode<T>
    {
        public T Item;
        public LinkedListNode<T> Before;
        public LinkedListNode<T> After;

        public void Clear()
        {
            Item = default;
            After = null;
            Before = null;
        }

        public LinkedListNode() { }
        public LinkedListNode(T item)
        {
            Item = item;
        }

        public void Remove()
        {
            if (Before is not null)
            {
                Before.After = After;
            }
            if (After is not null)
            {
                After.Before = Before;
            }
            Item = default;
        }
    }
}
