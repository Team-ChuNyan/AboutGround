using System.Diagnostics;
using UnityEngine;

public class MainSceneDebugger : MonoBehaviour
{
    public SeedMapData SeedData;
    public Vector2Int StartPos;
    public Vector2Int EndPos;

    public MapGenerator MapGenerator;
    public Pathfinding Pathfinding;

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

    public void FindPath()
    {
        _sw.Start();
        Pathfinding.FindPath(StartPos, EndPos);
        PrintStopWatch();
    }

    private void PrintStopWatch()
    {
        _sw.Stop();
        UnityEngine.Debug.Log(_sw.ElapsedMilliseconds + "ms");
        _sw.Reset();
    }
}