using System;
using UnityEngine;

public interface IMovable
{
    public Vector3 Position { get; }

    public event Action OnArrived;

    public void Move(Vector2Int pos);
    public void StopMovement();
    public void RegisterOnArrived(Action action);
    public void UnregisterOnArrived(Action action);
}
