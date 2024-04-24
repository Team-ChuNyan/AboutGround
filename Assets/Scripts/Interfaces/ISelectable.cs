using UnityEngine;

public interface ISelectable
{
    public bool IsSelection { get; }
    public void AddSelection();
    public void WaitSelection();
    public void CancelSelection();
    public Vector3 GetSelectPoint();
    public SelectPropType GetSelectPropType();
}
