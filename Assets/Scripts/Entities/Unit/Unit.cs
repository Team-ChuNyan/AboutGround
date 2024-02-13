using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IMovable
{
    public UnitData UnitData;
    public Dictionary<BodyPartType, BodyPart> BodyParts;

    protected List<PathNode> _movementPath;
    protected Coroutine _moveCoroutine;

    public virtual void Awake()
    {
        _movementPath = new List<PathNode>(64);
    }

    public void Move()
    {
        _moveCoroutine = StartCoroutine(FollowPathNode());
    }

    public void StopMovement()
    {
        StopCoroutine(_moveCoroutine);
        _movementPath.Clear();
    }

    protected IEnumerator FollowPathNode()
    {
        for (int i = 0; i < _movementPath.Count; i++)
        {
            Vector3 targetPos = new(_movementPath[i].Pos.x, _movementPath[i].Pos.y, 0);
            while (transform.position != targetPos)
            {
                var movePos = Time.deltaTime * UnitData.MoveSpeed * 10;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movePos);
                yield return null;
            }
        }

        _movementPath.Clear();
    }

    public Vector2Int GetCurrentPosition()
    {
        var vector3 = transform.position;
        return new Vector2Int((int)vector3.x, (int)vector3.y);
    }

    public List<PathNode> GetMovementPath()
    {
        return _movementPath;
    }
}