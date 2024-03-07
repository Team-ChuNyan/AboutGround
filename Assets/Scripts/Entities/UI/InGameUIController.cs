using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIController : MonoBehaviour
{
    private VisualElement root;
    private InteractionMenuUIHandler _interactionMenuUI;

    private const string InteractionScreen = "InteractionMenu";

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var childRoot = root.Q<VisualElement>(InteractionScreen);
        _interactionMenuUI = new (childRoot);
    }

    public EnumViewModel<InteractionType> GetInteractionMenuViewModel()
    {
        return _interactionMenuUI.GetViewModel();
    }
}
