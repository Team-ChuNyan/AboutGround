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
        GUILayout.Space(10);
        DrawCreatePlayerUnit();
        GUILayout.Space(10);
        DrawMoveUnit();
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

    private void DrawCreatePlayerUnit()
    {
        GUILayout.Label("CreatePlayerUnit", EditorStyles.boldLabel);
        _debugger.Name= EditorGUILayout.TextField("Name", _debugger.Name);
        _debugger.Race = (RaceType)EditorGUILayout.EnumPopup("RaceType", _debugger.Race);
        if (GUILayout.Button("Create"))
        {
            _debugger.CreatePlayerUnit();
        }
    }

    private void DrawMoveUnit()
    {
        GUILayout.Label("UnitMove", EditorStyles.boldLabel);
        _debugger.Unit = (Unit)EditorGUILayout.ObjectField("Unit", _debugger.Unit,typeof(Unit),true);
        _debugger.Goal = EditorGUILayout.Vector2IntField("EndPos", _debugger.Goal);
        if (GUILayout.Button("Move"))
        {
            _debugger.MoveUnit();
        }
    }
}