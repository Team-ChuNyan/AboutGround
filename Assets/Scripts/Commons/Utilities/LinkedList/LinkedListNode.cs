public class LinkedListNode<T> where T : class
{
    public T Item;
    public LinkedListNode<T> Before;
    public LinkedListNode<T> After;

    public void Clear()
    {
        Item = null;
        After = null;
        Before = null;
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
        Item = null;
    }
}
