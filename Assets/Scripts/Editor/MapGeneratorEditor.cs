using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.AutoUpdate == false)
                return;

            mapGen.Awake();
            mapGen.GenerateDisplayMap();
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.Awake();
            mapGen.GenerateDisplayMap();
        }

        if (GUILayout.Button("Generate&PaintTile"))
        {
            mapGen.Awake();
            mapGen.GenerateDisplayMap();
            mapGen.GeneratePathNodeMap();
            mapGen.PaintTileMap();
        }
    }
}