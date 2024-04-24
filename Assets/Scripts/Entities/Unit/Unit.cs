using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IMovable, ISelectable
{
    public UnitComponentHandler ComponentHandler;
    private IMoveSystem _moveSystem;

    protected UnitData _unitData;
    protected Dictionary<BodyPartType, BodyPart> _bodyParts;

    protected List<PathNode> _movementPath;
    protected Coroutine _moveCoroutine;
    private Vector2Int _targetPosition;

    private bool _isSelection;

    public event Action OnArrived;
    public bool IsArrive { get; set; }

    public static readonly List<InteractionType> PlayerUnitInteraction
    = new() { InteractionType.Cancel };

    public UnitData UnitData { get { return _unitData; } set { _unitData = value; } }
    public Dictionary<BodyPartType, BodyPart> BodyParts { get { return _bodyParts; } set { _bodyParts = value; } }
    public Vector3 Position { get { return transform.position; } }

    public virtual void Awake()
    {
        _movementPath = new List<PathNode>(64);
    }

    public void Move(Vector2Int goal)
    {
        _targetPosition = goal;
        Vector2Int currentPos = new((int)transform.position.x, (int)transform.position.z);
        _moveSystem.UpdateMovementPath(_movementPath, currentPos, goal);
        _moveCoroutine = StartCoroutine(FollowPathNode());
    }

    public void StopMovement()
    {
        if (_moveCoroutine is null)
            return;

        StopCoroutine(_moveCoroutine);
        _movementPath.Clear();
    }

    protected IEnumerator FollowPathNode()
    {
        IsArrive = false;
        for (int i = 0; i < _movementPath.Count; i++)
        {
            Vector3 targetPos = Util.Vector2IntToVector3(_movementPath[i].Pos);
            while (transform.position != targetPos)
            {
                var movePos = Time.deltaTime * UnitData.MoveSpeed * 10;
                var nextPos = Vector3.MoveTowards(transform.position, targetPos, movePos);
                transform.position = nextPos;
                yield return null;
            }
        }
        _movementPath.Clear();

        Vector3 goal = Util.Vector2IntToVector3(_targetPosition);
        if (goal == transform.position)
        {
            OnArrived?.Invoke();
            OnArrived = null;
            IsArrive = true;
        }
    }

    public void UnregisterOnArrived(Action action)
    {
        OnArrived -= action;
    }

    public void RegisterOnArrived(Action action)
    {
        OnArrived += action;
    }
    public void SetMoveSystem(IMoveSystem sys)
    {
        _moveSystem = sys;
    }

    public Vector3 GetSelectPoint()
    {
        return ComponentHandler.Collider.bounds.center;
    }

    public void AddSelection()
    {
        if (_isSelection == false)
        {
            ComponentHandler.SelectMaker.SetActive(true);
            _isSelection = true;
        }
        ComponentHandler.SelectMaker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void WaitSelection()
    {
        if (_isSelection == false)
        {
            ComponentHandler.SelectMaker.SetActive(true);
            _isSelection = true;
        }
        ComponentHandler.SelectMaker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public void CancelSelection()
    {
        ComponentHandler.SelectMaker.SetActive(false);
        _isSelection = false;
    }

    public SelectPropType GetSelectPropType()
    {
        return UnitData.SelectPropType;
    }
}
