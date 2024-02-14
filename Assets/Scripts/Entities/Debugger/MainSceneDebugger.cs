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
    public Pathfinding Pathfinding;

    public string Name;
    public RaceType Race;

    public Unit Unit;
    public Vector2Int Goal;

    private Stopwatch _sw;

    private void Awake()
    {
        SeedData = new SeedMapData();
        StartPos = new Vector2Int();
        EndPos = new Vector2Int();
        _sw = new();
    }

    private void Start()
    {
        Pathfinding = UnitController.Pathfinding;
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
        UnitController.MoveUnit(Unit, Goal);
    }

    public void StopMovementUnit()
    {
        UnitController.StopMovementUnit(Unit);
    }

    public void FindPath()
    {
        _sw.Start();
        Pathfinding.ReceiveMovementPath(new List<PathNode>(),StartPos, EndPos);
        PrintStopWatch();
    }

    private void PrintStopWatch()
    {
        _sw.Stop();
        UnityEngine.Debug.Log(_sw.ElapsedMilliseconds + "ms");
        _sw.Reset();
    }
}
