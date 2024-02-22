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
        DrawCreatePlayerUnit();
        GUILayout.Space(10);
        DrawMoveUnit();
        GUILayout.Space(10);
        DrawCreatePack();
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
        _debugger.MovableObject = (IMovable)EditorGUILayout.ObjectField("Unit", _debugger.MovableObject as MonoBehaviour,typeof(MonoBehaviour),true);
        _debugger.Goal = EditorGUILayout.Vector2IntField("EndPos", _debugger.Goal);
        if (GUILayout.Button("Move"))
        {
            _debugger.MoveUnit();
        }
        if (GUILayout.Button("Stop"))
        {
            _debugger.StopMovementUnit();
        }
    }

    private void DrawCreatePack()
    {
        GUILayout.Label("CreatePack", EditorStyles.boldLabel);
        _debugger.ItemType = (ItemType)EditorGUILayout.EnumPopup("ItemType", _debugger.ItemType);
        _debugger.CreatePackPosition = EditorGUILayout.Vector2IntField("CreatePos", _debugger.CreatePackPosition);
        _debugger.Amount = EditorGUILayout.IntField("Amount", _debugger.Amount);
        _debugger.Durability = EditorGUILayout.FloatField("Durability", _debugger.Durability);

        if (GUILayout.Button("Create"))
        {
            _debugger.CreateItemPack();
        }
    }
}
