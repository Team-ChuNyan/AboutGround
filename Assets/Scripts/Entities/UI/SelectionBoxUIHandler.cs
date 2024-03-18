using UnityEngine;
using UnityEngine.UIElements;

public class SelectionBoxUIHandler : UIBase
{
    private Vector3 _startPosition;

    public SelectionBoxUIHandler(VisualElement root) : base(root) { }

    public override void Show()
    {
        base.Show();
        _root.transform.scale = Vector3.zero;
    }

    public void SetStartPosition(Vector3 pos)
    {
        _startPosition = pos;

        Vector2 reference = InGameUIController.ReferenceResolution;
        float width = _startPosition.x / Screen.width * reference.x;
        float hight = reference.y - (_startPosition.y / Screen.height * reference.y);
        _root.transform.position = new Vector3(width, hight);
    }

    public void Refresh(Vector3 _endPos)
    {
        _root.transform.scale = _startPosition - _endPos;
    }
}
