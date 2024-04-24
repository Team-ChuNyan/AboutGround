using UnityEngine;

public interface IItemStorage
{
    public void KeepItem(IPackable item);
    public void PutDownItem(IPackable item, Vector3 pos, int amount);
}
