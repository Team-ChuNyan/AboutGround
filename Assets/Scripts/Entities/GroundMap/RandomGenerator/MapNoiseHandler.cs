using System.Collections.Generic;
using UnityEngine;

public class MapNoiseHandler
{
    private List<float> _noiseMap;
    public List<float> NoiseMap { get { return _noiseMap; } }

    public MapNoiseHandler()
    {
        _noiseMap = new();
    }

    private void ResetNoiseMap(SeedMapData seedData)
    {
        _noiseMap.Capacity = seedData.GetMapSize();
        _noiseMap.Clear();
    }

    public List<float> GenerateNoiseMap(SeedMapData seedData, NoiseData noiseData)
    {
        ResetNoiseMap(seedData);

        int height = seedData.Height;
        int width = seedData.Width;
        uint seed = (uint)seedData.Seed;
        var rand = new Unity.Mathematics.Random();
        rand.InitState(seed);

        Vector2[] octaveOffsets = new Vector2[noiseData.Octave];
        for (int i = 0; i < noiseData.Octave; i++)
        {
            float offsetX = rand.NextInt(-100000, 100000) + noiseData.Offset.x;
            float offsetY = rand.NextInt(-100000, 100000) + noiseData.Offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < noiseData.Octave; i++)
                {
                    float tempX = (x - halfWidth) / noiseData.NoiseScale * frequency + octaveOffsets[i].x;
                    float tempY = (y - halfHeight) / noiseData.NoiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(tempX, tempY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= noiseData.Persistance;
                    frequency *= noiseData.Lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                _noiseMap.Add(noiseHeight);
            }
        }

        for (int y = 0; y < height; y++)
        {
            int yValue = y * width;
            for (int x = 0; x < width; x++)
            {
                _noiseMap[yValue + x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, _noiseMap[yValue + x]);
            }
        }

        return _noiseMap;
    }
}
