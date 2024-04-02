using UnityEngine;

public interface ISelectable
{
    public void AddSelection();
    public void WaitSelection();
    public void CancelSelection();
    public Vector3 GetSelectPoint();
    public SelectPropType GetSelectPropType();
}
