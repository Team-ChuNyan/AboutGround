using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSceneDebugger))]
public class MainSceneDebuggerEditor : Editor
{
    private MainSceneDebugger _debugger;

    private void OnEnable()
    {
        _debugger = target as MainSceneDebugger;
    }

    public override void OnInspectorGUI()
    {
        DrawMapGenerator();
        GUILayout.Space(10);
        DrawPathfinding();
    }

    private void DrawMapGenerator()
    {
        GUILayout.Label("Map Generator", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        _debugger.SeedData.Width = EditorGUILayout.IntField("Width", _debugger.SeedData.Width);
        _debugger.SeedData.Height = EditorGUILayout.IntField("Height", _debugger.SeedData.Height);
        _debugger.SeedData.Seed = EditorGUILayout.IntField("Seed", _debugger.SeedData.Seed);
        if (GUILayout.Button("Map Generate"))
        {
            _debugger.GenerateNewMap();
        }

        EditorGUI.indentLevel--;
    }

    private void DrawPathfinding()
    {
        GUILayout.Label("Pathfinding", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        _debugger.StartPos= EditorGUILayout.Vector2IntField("StartPos", _debugger.StartPos);
        _debugger.EndPos = EditorGUILayout.Vector2IntField("EndPos", _debugger.EndPos);
        if (GUILayout.Button("Move"))
        {
            _debugger.FindPath();
        }

        EditorGUI.indentLevel--;
    }
}