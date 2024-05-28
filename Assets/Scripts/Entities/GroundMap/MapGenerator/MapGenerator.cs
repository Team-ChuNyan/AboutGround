using System.Collections.Generic;
using UnityEngine;

namespace AboutGround.GroundMap.Generator
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("일반")]
        private MapTextureGenerator _textureGenerator;
        private PathNodeMapGenerator _nodeMapGenerator;
        private MapNoiseHandler _noise;
        [SerializeField] private MapTextureDisplay _mapDisplay;
        private TilePainter _tilePainter;
        private List<float> _noiseMap;
        private Ground[,] _grounds;

        [Header("생성 방식")]
        [SerializeField] private SeedMapData _seedMap;
        [SerializeField] private NoiseData _noiseData;
        [SerializeField] private GeneratorGroundData[] _groundType;
        [SerializeField] private bool _isColorMap;

        public SeedMapData SeedMapInfo { get { return _seedMap; } set { _seedMap = value; } }
        public Ground[,] Grounds { get { return _grounds; } }

        public void Awake()
        {
            _noise = new MapNoiseHandler();
            _nodeMapGenerator = new PathNodeMapGenerator();
            _textureGenerator = new MapTextureGenerator(_groundType);
        }

        public MapGenerator Initialize(TilePainter tilePainter)
        {
            _tilePainter = tilePainter;
            return this;
        }

        public MapGenerator GenerateNoiseMap(SeedMapData seedmap)
        {
            _seedMap = seedmap;
            _noiseMap = _noise.GenerateNoiseMap(_seedMap, _noiseData);
            return this;
        }

        public MapGenerator GenerateDisplayMap()
        {
            Texture2D texture = _textureGenerator.TextureFromNoiseMap(_noiseMap, _seedMap, _isColorMap);
            _mapDisplay.DrawTexture(texture);
            return this;
        }

        public MapGenerator GeneratePathNodeMap()
        {
            _grounds = _nodeMapGenerator.GenerateNodePaths(_seedMap, _noiseMap, _groundType);
            return this;
        }

        public MapGenerator PaintTileMap()
        {
            if (SeedMapInfo.Equals(null)
                || _noiseMap.Equals(null))
            {
                Debug.LogError("null");
                return this;
            }

            _tilePainter.PaintGroundGrid(_seedMap, _noiseMap, _groundType);
            return this;
        }

        #region EditorMethod
#if (UNITY_EDITOR)
        private void OnValidate()
        {
            if (_noiseData.Lacunarity < 1)
                _noiseData.Lacunarity = 1;

            if (_noiseData.Octave < 0)
                _noiseData.Octave = 0;

            if (_noiseData.NoiseScale <= 0)
                _noiseData.NoiseScale = 0.0001f;

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
}
