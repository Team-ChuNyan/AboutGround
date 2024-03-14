using UnityEngine;

public interface ISelectable
{
    public void Select();
    public void CancelSelection();
    public Bounds GetSelectBounds();
}
