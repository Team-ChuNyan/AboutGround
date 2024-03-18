using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSelector
{
    private Camera _mainCam;
    private SelectionBoxUIHandler _selectionBoxUI;
    private List<Unit> _playerUnitProp;
    private HashSet<ISelectable> _currentSelection;
    private HashSet<ISelectable> _waitSelection;
    private HashSet<ISelectable> _beforeSelection;
    private PlayerInputController _input;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private int _selectableLayer;
    private bool _isSearching;

    public ObjectSelector()
    {
        _mainCam = Camera.main;
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
        PushCurrentSelection();
    }

    private void PerformSelection(Vector2 pos)
    {
        _endPosition = pos;
        SearchInsideUnit();
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
        ClearBeforeSelections();
        if (_waitSelection.Count > 0)
        {
            Util.Swap(ref _currentSelection, ref _waitSelection);

            foreach (var item in _currentSelection)
            {
                item.AddSelection();
            }
        }
        else
        {
            RaycastStartPosition();
        }
    }

    public void SearchInsideUnit()
    {
        Vector2 minPoint;
        Vector2 maxPoint;

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

        foreach (var item in _playerUnitProp)
        {
            bool isContains = _waitSelection.Contains(item);
            var point = item.GetSelectPoint();
            point = _mainCam.WorldToScreenPoint(point);

            if (Util.IsNumberInRange(point.x, minPoint.x, maxPoint.x) == true
             && Util.IsNumberInRange(point.y, minPoint.y, maxPoint.y) == true)
            {
                if (isContains == false)
                {
                    AddWaitObject(item);
                }
            }
            else if (isContains == true)
            {
                RemoveWaitObject(item);
            }
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

    private void PushCurrentSelection()
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
}
