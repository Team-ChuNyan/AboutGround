using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSelector
{
    public enum Mode { Default, Add }

    private Camera _mainCam;
    private SelectionBoxUIHandler _selectionBoxUI;
    private List<Unit> _playerUnitProp;

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


    public ObjectSelector()
    {
        _mainCam = Camera.main;
        _selectMode = Mode.Default;
        _playerUnitProp = new(16);
        _waitSelection = new(16);
        _currentSelection = new(16);
        _beforeSelection = new(16);
        _selectableLayer = Const.Layer_Selectable;
    }

    public void Init(PlayerInputController con, SelectionBoxUIHandler ui)
    {
        _input = con;
        _selectionBoxUI = ui;
        con.RegisterClickStarted(StartSelection);
        con.RegisterClickCanceled(CancelSelection);

        con.RegisterShiftPressed(ToggleAddMode);
    }

    public void SetTargetProp(List<Unit> target)
    {
        _playerUnitProp = target;
    }

    private void StartSelection()
    {
        if (_isSearching == false)
        {
            _input.RegisterMoveMousePerformed(PerformSelection);
            _isSearching = true;
        }

        _startPosition = Mouse.current.position.ReadValue();
        _selectionBoxUI.Show();
        _selectionBoxUI.SetStartPosition(_startPosition);
        SwapCurrentSelection();
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
        if (_isSearching == true)
        {
            _input.UnregisterMoveMousePerformed(PerformSelection);
            _isSearching = false;
        }

        _selectionBoxUI.Hide();
        MergeBeforeSelectionToCurrentSelection();
        ClearBeforeSelections();
        DecideSelection();
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

    public void HandleSeletion()
    {
        foreach (var item in _playerUnitProp)
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
        else
        {
            RaycastStartPosition();
        }
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

    private void SwapCurrentSelection()
    {
        _beforeSelection.Clear();
        Util.Swap(ref _beforeSelection, ref _currentSelection);
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

    private void MergeBeforeSelectionToCurrentSelection()
    {
        if (_selectMode != Mode.Add)
            return;

        foreach (var item in _beforeSelection)
        {
            AddCurrentObject(item);
        }
    }

    private void ToggleAddMode(bool isOn)
    {
        _selectMode = isOn == true ? Mode.Add : Mode.Default;
    }
}
