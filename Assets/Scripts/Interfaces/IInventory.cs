using UnityEngine;

public interface IInventory
{
    public void KeepItem(IPackable item);
    public void PutDownItem(IPackable item, Vector3 pos, int amount);
}
