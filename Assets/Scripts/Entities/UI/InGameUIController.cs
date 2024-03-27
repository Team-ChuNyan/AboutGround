using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIController : MonoBehaviour
{
    public static readonly Vector2 ReferenceResolution = new(1920, 1080);

    public InteractionMenuUIHandler InteractionMenuUI { get; private set; }
    public SelectionBoxUIHandler DragSelectionUI { get; private set; }

    private VisualElement root;

    private const string InteractionMenu = "InteractionMenu";
    private const string SelectionBox = "SelectionBox";

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var childRoot = root.Q<VisualElement>(InteractionMenu);
        var dragSelectionRoot = root.Q<VisualElement>(SelectionBox);

        InteractionMenuUI = new(childRoot);
        DragSelectionUI = new(dragSelectionRoot);
    }

    public EnumViewModel<InteractionType> GetInteractionMenuViewModel()
    {
        return InteractionMenuUI.GetViewModel();
    }
}
