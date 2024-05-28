using System.Collections.Generic;
using UnityEngine;

namespace AboutGround.GroundMap.Generator
{
    public class TilePainter : MonoBehaviour
    {
        private Transform _rootTransform;
        private GroundTile _groundTile;

        private List<Mesh> _meshes;
        private List<Material> _materials;

        private enum FileOrder
        {
            Soil, Sand, Rock
        }

        private void Awake()
        {
            _rootTransform = new GameObject().transform;
            _rootTransform.name = "Tiles";
            _meshes = new();
            _materials = new();
            _groundTile = Resources.Load<GroundTile>("Prefabs/GroundTile");

            _meshes.Add(Resources.Load<Mesh>("Models/Ground/Soil/Mesh"));
            _materials.Add(Resources.Load<Material>("Models/Ground/Soil/Material"));

            _meshes.Add(Resources.Load<Mesh>("Models/Ground/Sand/Mesh"));
            _materials.Add(Resources.Load<Material>("Models/Ground/Sand/Material"));

            _meshes.Add(Resources.Load<Mesh>("Models/Ground/Rock/Mesh"));
            _materials.Add(Resources.Load<Material>("Models/Ground/Rock/Material"));
        }

        public void PaintGroundGrid(SeedMapData data, List<float> noise, GeneratorGroundData[] type)
        {
            var mapSize = data.GetMapSize() - 1;
            for (int height = 0; height < data.Height; height++)
            {
                for (int width = 0; width < data.Width; width++)
                {
                    float currentHeight = noise[mapSize - (height * data.Width + width)];
                    for (int i = 0; i < type.Length; i++)
                    {
                        if (currentHeight <= type[i].height)
                        {
                            GenerateTileObject(new Vector2Int(width, height), type[i].Type);
                            break;
                        }
                    }
                }
            }
        }

        private void GenerateTileObject(Vector2Int pos, GroundType type)
        {
            int orderType;
            switch (type)
            {
                case GroundType.Water:
                    return;
                case GroundType.Grass:
                    orderType = (int)FileOrder.Soil;
                    break;
                case GroundType.Ground:
                    orderType = (int)FileOrder.Sand;
                    break;
                case GroundType.RockField:
                    orderType = (int)FileOrder.Rock;
                    break;
                default:
                    Debug.LogError("TileMatching Failed");
                    return;
            }
            GroundTile newObj = Instantiate(_groundTile, _rootTransform);
            newObj.transform.position = new Vector3(pos.x, -1, pos.y);
            newObj.SetMesh(_meshes[orderType]);
            newObj.SetMaterial(_materials[orderType]);

            if (type == GroundType.RockField)
            {
                newObj = Instantiate(_groundTile, _rootTransform);
                newObj.transform.position = new Vector3(pos.x, 0, pos.y);
                newObj.SetMesh(_meshes[orderType]);
                newObj.SetMaterial(_materials[orderType]);
            }
        }
    }
}
