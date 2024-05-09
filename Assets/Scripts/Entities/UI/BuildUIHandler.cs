using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BuildUIHandler : UIBase, ICancelable
{
    private IconButton _cancel;
    private QuickCanceling _quickCanceling;

    private VisualElement _itemListUI;
    private List<IconButton<BuildingType>> _itemButtons;

    private VisualElement _materialUI;
    private List<IconButton<int>> _materials;

    private VisualElement _informationUI;
    private VisualElement _textArea;
    private VisualElement _resourceText;
    private VisualElement _descText;
    private VisualElement _effectListText;

    private event Action<BuildingType> ItemClicked;

    #region Const
    private const string CancelButtonObjName = "CancelButton";

    private const string ItemListUIObjName = "ItemList";
    private const string ItemButtonObjName = "ItemButton0";

    private const string MaterialUIObjName = "Material";
    private const string MaterialObjName = "Material0";

    private const string InformationUIObjName = "Information";
    private const string TextAreaObjName = "TextArea";
    private const string ResourceTextObjName = "ResourceText";
    private const string DescTextObjName = "DescText";
    private const string EffectListTextObjName = "EffectListText";
    #endregion

    public BuildUIHandler(VisualElement root, QuickCanceling canceling) : base(root)
    {
        _cancel.RegisterEvent(Hide);
        _quickCanceling = canceling;
    }

    public override void Show()
    {
        base.Show();
        _quickCanceling.Push(this);
    }

    public override void Hide()
    {
        base.Hide();
        _quickCanceling.Remove(this);
    }

    protected override void SetVisualElement()
    {
        _materials = new();
        _itemButtons = new();

        var buttonElement = _root.Q<VisualElement>(CancelButtonObjName);
        var button = new IconButton(buttonElement);
        _cancel = button;

        _itemListUI = _root.Q<VisualElement>(ItemListUIObjName);
        buttonElement = _itemListUI.Q<VisualElement>(ItemButtonObjName);
        var buildingTypeButton = new IconButton<BuildingType>(buttonElement);
        buildingTypeButton.RegisterEvent(OnItemClicked);
        _itemButtons.Add(buildingTypeButton);

        _materialUI = _root.Q<VisualElement>(MaterialUIObjName);
        buttonElement = _materialUI.Q<VisualElement>(MaterialObjName);
        var IntButton = new IconButton<int>(buttonElement);
        _materials.Add(IntButton);

        _informationUI = _root.Q<VisualElement>(InformationUIObjName);
        _textArea = _informationUI.Q<VisualElement>(TextAreaObjName);
        _resourceText = _informationUI.Q<VisualElement>(ResourceTextObjName);
        _descText = _informationUI.Q<VisualElement>(DescTextObjName);
        _effectListText = _informationUI.Q<VisualElement>(EffectListTextObjName);
    }

    private void OnItemClicked(BuildingType type)
    {
        ItemClicked?.Invoke(type);
    }

    public void QuickCancel()
    {
        Hide();
    }

    public bool IsCanceled()
    {
        return _root.style.display == DisplayStyle.None;
    }

    public void RegisterItemClicked(Action<BuildingType> action)
    {
        ItemClicked += action;
    }
}
