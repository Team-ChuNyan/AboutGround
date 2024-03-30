using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PropSelecting : ICancelable
{
    // TODO : 선택 방식을 레이캐스트로 통일
    public enum Mode { Default, Add, Cancel }

    private Camera _mainCam;
    private SelectionBoxUIHandler _selectionBoxUI;
    private QuickCanceling _quickCanceling;
    private List<Unit> _playerHumans;

    private Mode _selectMode;
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

    public PropSelecting()
    {
        _mainCam = Camera.main;
        _selectMode = Mode.Default;
        _playerHumans = new(16);
        _waitSelection = new(16);
        _currentSelection = new(16);
        _beforeSelection = new(16);
        _selectableLayer = Const.Layer_Selectable;
    }

    public void Init(PlayerInputController con, SelectionBoxUIHandler ui, QuickCanceling quickCanceling, PropsContainer props)
    {
        _input = con;
        _selectionBoxUI = ui;
        _quickCanceling = quickCanceling;
        _playerHumans = props.PlayerUnits[RaceType.Human];

        con.RegisterClickStarted(StartSelection);
        con.RegisterClickCanceled(CancelSelection);

        con.RegisterShiftPressed(ToggleAddMode);
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
        _selectMode = isOn == true ? Mode.Add : Mode.Default;
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
        _selectMode = Mode.Cancel;
        ClearWaitSelections();
        CancelSelection();
        _selectMode = Mode.Default;
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
        foreach (var item in _playerHumans)
        {
            bool isContains = _waitSelection.Contains(item);
            var point = item.GetSelectPoint();
            point = _mainCam.WorldToScreenPoint(point);

            if (Util.IsNumberInRange(point.x, minPoint.x, maxPoint.x) == true
             && Util.IsNumberInRange(point.y, minPoint.y, maxPoint.y) == true)
            {
                if (isContains == true)
                    continue;

                AddWaitObject(item);
            }
            else if (isContains == true)
            {
                RemoveWaitObject(item);
            }
        }
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
    }

    private void ClickSelection()
    {
        if (_startPosition != _endPosition)
            return;

        RaycastStartPosition();
    }

    private void RaycastStartPosition()
    {
        Ray ray = _mainCam.ScreenPointToRay(_startPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _selectableLayer) == true)
        {
            var obj = hitInfo.collider.gameObject.GetComponent<ISelectable>();
            AddCurrentObject(obj);
        }
    }

    private void AddCurrentObject(ISelectable obj)
    {
        _currentSelection.Add(obj);
        obj.AddSelection();
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
        if (_selectMode == Mode.Default)
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
        _currentSelection.Clear();
    }

    private void OnChangeSelection()
    {
        if (_selectMode == Mode.Cancel)
            return;

        SelectionChanged?.Invoke();
    }
}
