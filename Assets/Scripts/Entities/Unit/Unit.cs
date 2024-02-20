using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IMovable
{
    private IMoveSystem _moveSystem;

    public UnitData UnitData;
    public Dictionary<BodyPartType, BodyPart> BodyParts;

    protected List<PathNode> _movementPath;
    protected Coroutine _moveCoroutine;

    public event Action OnArrived;

    public virtual void Awake()
    {
        _movementPath = new List<PathNode>(64);
    }

    public void Move(Vector2Int goal)
    {
        Vector2Int currentPos = new((int)transform.position.x, (int)transform.position.y);
        _moveSystem.UpdateMovementPath(_movementPath, currentPos, goal);
        _moveCoroutine = StartCoroutine(FollowPathNode());
    }

    public void StopMovement()
    {
        StopCoroutine(_moveCoroutine);
        OnArrived = null;
        _movementPath.Clear();
    }

    protected IEnumerator FollowPathNode()
    {
        for (int i = 0; i < _movementPath.Count; i++)
        {
            Vector3 targetPos = Util.Vector2IntToVector3(_movementPath[i].Pos);
            while (transform.position != targetPos)
            {
                var movePos = Time.deltaTime * UnitData.MoveSpeed * 10;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movePos);
                yield return null;
            }
        }
        _movementPath.Clear();
        OnArrived?.Invoke();
        OnArrived = null;
    }

    public void RegisterOnArrived(Action action)
    {
        OnArrived += action;
    }

    public void UnregisterOnArrived(Action action)
    {
        OnArrived -= action;
    }

    public void SetMoveSystem(IMoveSystem sys)
    {
        _moveSystem = sys;
    }
}
