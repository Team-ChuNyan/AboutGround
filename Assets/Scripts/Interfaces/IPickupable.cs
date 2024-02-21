using UnityEngine;

public interface IPickupable
{
    public void CreateCarryWork(int amount);
    public void PickUp(int amount);
    public void PutDown(int amount);
}
 