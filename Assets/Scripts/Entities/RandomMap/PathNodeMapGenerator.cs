using System.Collections.Generic;
using UnityEngine;

public class PathNodeMapGenerator
{
    private float _blockMinValue;
    private float _blockMaxValue;

    public PathNode[,] GenerateNodePaths(SeedMapData seedMap, List<float> noiseMap, GeneratorGroundType[] type)
    {
        PathNode[,] _pathNodes = InitializePathNodes(seedMap.Width, seedMap.Height);
        FindRockField(type);

        var mapSize = seedMap.GetMapSize() - 1;

        for (int height = 0; height < seedMap.Height; height++)
        {
            for (int width = 0; width < seedMap.Width; width++)
            {
                float currentHeight = noiseMap[mapSize - (height * seedMap.Width + width)];

                if (currentHeight <= _blockMinValue || _blockMaxValue < currentHeight)
                    continue;

                _pathNodes[width, height].IsBlocked = true;
            }
        }

        return _pathNodes;
    }

    private PathNode[,] InitializePathNodes(int width, int height)
    {
        PathNode[,] pathNodes = new PathNode[width, height];
        for (int i = 0; i < pathNodes.GetLength(0); i++)
        {
            for (int k = 0; k < pathNodes.GetLength(1); k++)
            {
                pathNodes[i, k] = new PathNode(new Vector2Int(i, k));
            }
        }

        return pathNodes;
    }

    private void FindRockField(GeneratorGroundType[] type)
    {
        for (int i = 0; i < type.Length; i++)
        {
            if (type[i].Type != GroundType.RockField)
                continue;

            if (i == 0)
            {
                _blockMinValue = 0;
                _blockMaxValue = type[i].height;
            }
            else
            {
                _blockMinValue = type[i-1].height;
                _blockMaxValue = type[i].height;
            }

            break;
        }
    }
}