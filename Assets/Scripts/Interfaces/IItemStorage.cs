using UnityEngine;

public interface IItemStorage : IInventory
{
    public Vector3 Position { get; }
}
