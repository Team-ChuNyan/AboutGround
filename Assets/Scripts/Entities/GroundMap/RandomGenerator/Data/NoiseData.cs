using UnityEngine;

[System.Serializable]
public struct NoiseData
{
    public float NoiseScale;
    public int Octave;
    [Range(0f, 1f)] public float Persistance;
    public float Lacunarity;
    public Vector2 Offset;
}