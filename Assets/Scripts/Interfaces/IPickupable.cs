using UnityEngine;

public interface IPickupable
{
    public IPackable Item { get; }
    public bool IsGenerateCarry { get; set; }

    public Vector3 Position { get; }
    public void CreateCarryWork(int amount);
    public void PickUp(IWorkable worker, int amount);
    public void FinishWork(WorkType type); // TODO : Flag로 끄기
}
 