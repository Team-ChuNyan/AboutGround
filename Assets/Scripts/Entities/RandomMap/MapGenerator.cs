using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("일반")]
    private MapTextureGenerator _textureGenerator;
    private MapNoiseHandler _noise;
    [SerializeField] private MapTextureDisplay _mapDisplay;

    [Header("생성 방식")]
    [SerializeField] private SeedMapData _seedMap;
    [SerializeField] private bool _isColorMap;
    [SerializeField] private float _noiseScale;
    [SerializeField] private int _octave;
    [SerializeField][Range(0f, 1f)] private float _persistance;
    [SerializeField] private float _lacunarity;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private bool _autoUpdate;
    [SerializeField] private GeneratorGroundType[] _groundType;

    public SeedMapData SeedMapInfo { get { return _seedMap; } set { _seedMap = value; } }
    public bool AutoUpdate { get { return _autoUpdate; } }

    public void Awake()
    {
        _noise = new MapNoiseHandler();
        _textureGenerator = new MapTextureGenerator(_groundType);
    }

    public MapGenerator SetSeedMapInfo(SeedMapData seedmap)
    {
        _seedMap = seedmap;
        return this;
    }

    public void GenerateMap()
    {
        var noiseMap = _noise.GenerateNoiseMap(_seedMap, _noiseScale, _octave, _persistance, _lacunarity, _offset);
        Texture2D texture = _textureGenerator.TextureFromNoiseMap(noiseMap, SeedMapInfo,_isColorMap);
        _mapDisplay.DrawTexture(texture);
    }

    #region EditorMethod
#if (UNITY_EDITOR)
    private void OnValidate()
    {
        if (_lacunarity < 1)
            _lacunarity = 1;

        if (_octave < 0)
            _octave = 0;

        if (_noiseScale <= 0)
            _noiseScale = 0.0001f;

        if (_seedMap.Seed <= 0)
            _seedMap.Seed = 1;

        if (_seedMap.Width <= 0)
            _seedMap.Width = 0;

        if (_seedMap.Height <= 0)
            _seedMap.Height = 0;
    }
#endif
    #endregion
}