using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void Move();
    public void StopMovement();
    public Vector2Int GetCurrentPosition();
    public List<PathNode> GetMovementPath();
}