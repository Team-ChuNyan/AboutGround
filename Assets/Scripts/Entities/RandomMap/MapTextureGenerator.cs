using System.Collections.Generic;
using UnityEngine;

public class MapTextureGenerator
{
    private GeneratorGroundType[] _regions;
    private Color32[] _colorMap;
    private Texture2D _texture;

    public MapTextureGenerator(GeneratorGroundType[] regions)
    {
        _regions = regions;

        _colorMap = new Color32[256 * 256];
        _texture = new Texture2D(256, 256)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
    }

    public Texture2D TextureFromNoiseMap(List<float> noiseMap, SeedMapData seedMap, bool isColor)
    {
        SetTextureSize(seedMap);

        if (isColor)
        {
            SetColorMapUsingRegion(noiseMap, seedMap);
        }
        else
        {
            SetNoiseMapUsingGrayScale(noiseMap, seedMap);
        }

        _texture.SetPixels32(_colorMap);
        _texture.Apply();
        return _texture;
    }

    private void SetTextureSize(SeedMapData seedMap)
    {
        if (seedMap.Width != _texture.width || seedMap.Height != _texture.height)
        {
            _texture.Reinitialize(seedMap.Width, seedMap.Height);
            _colorMap = new Color32[seedMap.GetMapSize()];
        }
    }

    private void SetNoiseMapUsingGrayScale(List<float> noiseMap, SeedMapData seedMap)
    {
        int width = seedMap.Width;
        int height = seedMap.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _colorMap[y * width + x] = Color32.Lerp(Color.black, Color.white, noiseMap[y * width + x]);
            }
        }
    }

    private void SetColorMapUsingRegion(List<float> noiseMap, SeedMapData seedMap)
    {
        int height = seedMap.Height;
        int width = seedMap.Width;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = noiseMap[y * width + x];
                for (int i = 0; i < _regions.Length; i++)
                {
                    if (currentHeight <= _regions[i].height)
                    {
                        _colorMap[y * width + x] = _regions[i].color;
                        break;
                    }
                }
            }
        }
    }
}
