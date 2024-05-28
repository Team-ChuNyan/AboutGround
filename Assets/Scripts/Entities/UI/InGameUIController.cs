using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIController : MonoBehaviour
{
    public static readonly Vector2 ReferenceResolution = new(1920, 1080);

    public InteractionMenuUIHandler InteractionMenuUI { get; private set; }
    public SelectionBoxUIHandler DragSelectionUI { get; private set; }
    public ManagementMenuUIHandler ManagementMenuUI { get; private set; }
    public BuildUIHandler BuildUI { get; private set; }

    private VisualElement root;

    #region Const
    private const string InteractionMenu = "InteractionMenu";
    private const string SelectionBox = "SelectionBox";
    private const string ManagementMenu = "ManagementMenu";
    private const string Build = "Build";
    #endregion

    public void Init(QuickCanceling quickCanceling)
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var childRoot = root.Q<VisualElement>(InteractionMenu);
        var dragSelectionRoot = root.Q<VisualElement>(SelectionBox);
        var managementMenu = root.Q<VisualElement>(ManagementMenu);
        var build = root.Q<VisualElement>(Build);

        InteractionMenuUI = new(childRoot);
        DragSelectionUI = new(dragSelectionRoot);
        ManagementMenuUI = new(managementMenu);
        BuildUI = new(build, quickCanceling);

        ManagementMenuUI.RegisterButtonEvent(ManagementMenuUIHandler.ButtonType.Build, BuildUI.Show);
    }
}
