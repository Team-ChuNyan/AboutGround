using UnityEngine;

public interface IPackable
{
    public bool IsFull { get; }
    public bool IsStack {  get;}
    public int Amount { get; set; }
    public int MaxAmount { get; }

    public void StackItem(IPackable item);
    public void Destroy();
    public IPackable CopyPack();
    public void Pack(Vector2Int respawn);
}
