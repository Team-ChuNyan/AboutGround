using System.Collections.Generic;
using UnityEngine;

public interface IMoveSystem
{
    public void UpdateMovementPath(List<PathNode> path, Vector2Int start, Vector2Int goal);
}
