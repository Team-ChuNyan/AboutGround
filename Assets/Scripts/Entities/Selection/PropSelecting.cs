using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PropSelecting : ICancelable
{
    public enum Mode { Default, Add, Cancel }

    private Camera _mainCam;
    private SelectionBoxUIHandler _selectionBoxUI;
    private QuickCanceling _quickCanceling;
    private List<Unit> _playerHumans;
    private List<Pack> _packs;

    private Mode _mode;
    private SelectPropType _selectType;
    private SelectPropType _beforeType;
    private HashSet<ISelectable> _currentSelection;
    private HashSet<ISelectable> _waitSelection;
    private HashSet<ISelectable> _beforeSelection;
    private PlayerInputController _input;
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private readonly int _selectableLayer;
    private bool _isSearching;
    private Vector2 minPoint;
    private Vector2 maxPoint;

    private event Action SelectionChanged;

    public SelectPropType SelectType { get { return _selectType; } }
    public HashSet<ISelectable> CurrentSelection { get { return _currentSelection; } }

    public PropSelecting()
    {
        _mainCam = Camera.main;
        _mode = Mode.Default;
        _playerHumans = new(16);
        _waitSelection = new(16);
        _currentSelection = new(16);
        _beforeSelection = new(16);
        _selectableLayer = Const.Layer_Selectable;

        PackGenerator.Instance.RegisterDestroyed(RemoveCurrentObject);
    }

    public void Init(SelectionBoxUIHandler ui, QuickCanceling quickCanceling, PropsContainer props)
    {
        _selectionBoxUI = ui;
        _quickCanceling = quickCanceling;
        _playerHumans = props.PlayerUnits[RaceType.Human];
        _packs = props.Packs;
    }

    public void InitInput(PlayerInputController con, MouseInputHandler mouseInputHandler)
    {
        _input = con;
        _input.RegisterShiftPressed(ToggleAddMode);

        mouseInputHandler.RegisterClickStarted(MouseInputHandler.LeftClick.Selecting, StartSelection);
        mouseInputHandler.RegisterClickCanceled(MouseInputHandler.LeftClick.Selecting, CancelSelection);

    }

    public HashSet<ISelectable> GetSelection()
    {
        return _currentSelection;
    }

    public bool IsCanceled()
    {
        return _isSearching == false;
    }

    private void ToggleAddMode(bool isOn)
    {
        if (isOn == true)
        {
            _mode = Mode.Add;
            if (_isSearching == true && _selectType != _beforeType)
            {
                _selectType = _beforeType;
                ClearWaitSelections();
                HandleSeletion();
            }
        }
        else
        {
            _mode = Mode.Default;
        }
    }

    public void RegisterSelectionChanged(Action action)
    {
        SelectionChanged += action;
    }

    private void StartSelection()
    {
        if (_isSearching == false)
        {
            _quickCanceling.Push(this);
            _input.RegisterMoveMousePerformed(PerformSelection);
            _isSearching = true;
        }

        _startPosition = Mouse.current.position.ReadValue();
        _endPosition = _startPosition;
        _selectionBoxUI.Show();
        _selectionBoxUI.SetStartPosition(_startPosition);
        _beforeSelection.Clear();
        MergeCurnnetSelectionToBeforeSelection();
    }

    private void PerformSelection(Vector2 pos)
    {
        _endPosition = pos;
        CalcMinMaxPoint();
        HandleSeletion();
        _selectionBoxUI.Refresh(_endPosition);
    }

    private void CancelSelection()
    {
        if (_isSearching == false)
            return;

        _quickCanceling.Remove(this);
        UnregisterPerformSelection();
        MergeBeforeSelectionToCurrentSelection();
        ClearBeforeSelections();
        ClickSelection();
        DecideSelection();
        OnChangeSelection();
    }

    public void QuickCancel()
    {
        _mode = Mode.Cancel;
        ClearWaitSelections();
        CancelSelection();
        _mode = Mode.Default;
    }

    private void UnregisterPerformSelection()
    {
        _input.UnregisterMoveMousePerformed(PerformSelection);
        _selectionBoxUI.Hide();
        _isSearching = false;
    }

    private void CalcMinMaxPoint()
    {
        if (_startPosition.x < _endPosition.x)
        {
            minPoint.x = _startPosition.x;
            maxPoint.x = _endPosition.x;
        }
        else
        {
            minPoint.x = _endPosition.x;
            maxPoint.x = _startPosition.x;
        }

        if (_startPosition.y < _endPosition.y)
        {
            minPoint.y = _startPosition.y;
            maxPoint.y = _endPosition.y;
        }
        else
        {
            minPoint.y = _endPosition.y;
            maxPoint.y = _startPosition.y;
        }
    }

    private void HandleSeletion()
    {
        if (_mode == Mode.Default)
        {
            if (TryFindPropsInSelectionBox(_playerHumans))
                _selectType = SelectPropType.PlayerUnit;
            else if (TryFindPropsInSelectionBox(_packs))
                _selectType = SelectPropType.Pack;
        }
        else if (_mode == Mode.Add)
        {
            if (_selectType == SelectPropType.PlayerUnit)
                TryFindPropsInSelectionBox(_playerHumans);
            else if (_selectType == SelectPropType.Pack)
                TryFindPropsInSelectionBox(_packs);
        }
    }

    private bool TryFindPropsInSelectionBox<T>(List<T> props) where T : ISelectable
    {
        int find = 0;
        foreach (var item in props)
        {
            var point = item.GetSelectPoint();
            point = _mainCam.WorldToScreenPoint(point);

            if (Util.IsNumberInRange(point.x, minPoint.x, maxPoint.x) == true
             && Util.IsNumberInRange(point.y, minPoint.y, maxPoint.y) == true)
            {
                if (_waitSelection.Contains(item) == false)
                    AddWaitObject(item);

                find++;
            }
            else if (_waitSelection.Contains(item) == true)
            {
                RemoveWaitObject(item);
            }
        }

        return find > 0;
    }

    private void DecideSelection()
    {
        if (_waitSelection.Count > 0)
        {
            foreach (var item in _waitSelection)
            {
                if (_currentSelection.Contains(item) == true)
                    continue;

                AddCurrentObject(item);
            }
            _waitSelection.Clear();
        }

        if (_currentSelection.Count == 0)
        {
            _selectType = SelectPropType.None;
        }
    }

    private void ClickSelection()
    {
        float distance = (_startPosition - _endPosition).sqrMagnitude;
        if (distance < 500f)
        {
            RaycastStartPosition();
        }
    }

    public void UnselectAllSelection()
    {
        foreach (var item in _currentSelection)
        {
            item.CancelSelection();
        }
        _currentSelection.Clear();

        OnChangeSelection();
    }

    private void RaycastStartPosition()
    {
        Ray ray = _mainCam.ScreenPointToRay(_startPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _selectableLayer) == true)
        {
            var obj = hitInfo.collider.gameObject.GetComponent<ISelectable>();
            _selectType = obj.GetSelectPropType();
            AddCurrentObject(obj);
        }
    }

    private void AddCurrentObject(ISelectable obj)
    {
        _currentSelection.Add(obj);
        obj.AddSelection();
    }

    private void RemoveCurrentObject(ISelectable obj)
    {
        if (obj.IsSelection == false)
            return;

        _currentSelection.Remove(obj);
        OnChangeSelection();
    }

    private void AddWaitObject(ISelectable obj)
    {
        _waitSelection.Add(obj);
        obj.WaitSelection();
    }

    private void RemoveWaitObject(ISelectable obj)
    {
        if (_beforeSelection.Contains(obj))
        {
            obj.AddSelection();
        }
        else
        {
            obj.CancelSelection();
        }
        _waitSelection.Remove(obj);
    }

    private void ClearBeforeSelections()
    {
        foreach (var item in _beforeSelection)
        {
            if (_currentSelection.Contains(item) == false)
            {
                item.CancelSelection();
            }
        }
        _beforeSelection.Clear();
    }

    private void ClearWaitSelections()
    {
        foreach (var item in _waitSelection)
        {
            item.CancelSelection();
        }
        _waitSelection.Clear();
    }

    private void MergeBeforeSelectionToCurrentSelection()
    {
        if (_mode == Mode.Default
         || _selectType != _beforeType)
            return;

        foreach (var item in _beforeSelection)
        {
            AddCurrentObject(item);
        }
    }

    private void MergeCurnnetSelectionToBeforeSelection()
    {
        foreach (var item in _currentSelection)
        {
            _beforeSelection.Add(item);
        }
        _beforeType = _selectType;
        _currentSelection.Clear();
    }

    private void OnChangeSelection()
    {
        if (_mode == Mode.Cancel)
            return;

        SelectionChanged?.Invoke();
    }
}
