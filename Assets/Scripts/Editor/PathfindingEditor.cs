using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathfinding))]
public class PathfindingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            var pathfinding = (Pathfinding)target;
            pathfinding.Awake();

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            pathfinding.FindPath(pathfinding.Start, pathfinding.Goal);
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
        }
    }
}
