using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MainSceneDebugger : MonoBehaviour
{
    public SeedMapData SeedData;
    public Vector2Int StartPos;
    public Vector2Int EndPos;

    public MapGenerator MapGenerator;
    public UnitController UnitController;
    public GroundPathfinding Pathfinding;

    public string Name;
    public RaceType Race;

    public IMovable MovableObject;
    public Vector2Int Goal;

    private Stopwatch _sw;

    private void Awake()
    {
        SeedData = new SeedMapData();
        StartPos = new Vector2Int();
        EndPos = new Vector2Int();
        _sw = new();
    }

    public void GenerateNewMap()
    {
        MapGenerator.GenerateNoiseMap(SeedData)
            .GenerateDisplayMap()
            .GeneratePathNodeMap()
            .PaintTileMap();
    }

    public void CreatePlayerUnit()
    {
        UnitController.CreateNewPlayerUnit(RaceType.Human);
    }

    public void MoveUnit()
    {
        Action action = () => { UnityEngine.Debug.Log("도착"); };
        MovableObject.RegisterOnArrived(action);
        MovableObject.Move(Goal);
    }

    public void StopMovementUnit()
    {
        MovableObject.StopMovement();
    }
}
