using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private TileBase _ground;
    [SerializeField] private TileBase _rockField;


    public void PaintGroundGrid(SeedMapData data, List<float> noise, GeneratorGroundType[] type)
    {
        _tileMap.ClearAllTiles();
        var mapSize = data.GetMapSize()-1;
        for (int height = 0; height < data.Height; height++)
        {
            for (int width = 0; width < data.Width; width++)
            {
                float currentHeight = noise[mapSize -( height * data.Width + width)];
                for (int i = 0; i < type.Length; i++)
                {
                    if (currentHeight <= type[i].height)
                    {
                        _tileMap.SetTile(new Vector3Int(width, height), SwitchTileBase(type[i].Type));
                        break;
                    }
                }
            }
        }
    }

    private TileBase SwitchTileBase(GroundType type)
    {
        switch (type)
        {
            case GroundType.Ground:
                return _ground;
            case GroundType.RockField:
                return _rockField;
            case GroundType.Rock:
            default:
                return _rockField;
        }
    }
}
