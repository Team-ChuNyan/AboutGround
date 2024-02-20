using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneType _currentScene;
    [SerializeField] private GameObject _debugger;

    private void Awake()
    {
        gameObject.name = "GameController";
        InitializeScene(_currentScene);
        Destroy(this);
    }

    private void InitializeScene(SceneType type)
    {
        switch (type)
        {
            case SceneType.Title:
                break;
            case SceneType.Main:
                LoadMainScene();
                break;
            case SceneType.Loading:
                break;
        }
    }

    private void LoadMainScene()
    {
        new DataManager().InitializeItemData();

        SeedMapData seed = new(100, 100, 9123);

        var inputController = gameObject.AddComponent<PlayerInputController>();
        var unitController = gameObject.AddComponent<UnitController>();
        var itemController = gameObject.AddComponent<ItemController>();

        var mapGenerator = Instantiate(Resources.Load<MapGenerator>("Prefabs/MapGenerator"));
        var GroundGiud = Instantiate(Resources.Load<TilePainter>("Prefabs/GroundGrid"));
        mapGenerator.name = "MapGenerator";
        GroundGiud.name = "GrundGrid";

        mapGenerator.Initialize(GroundGiud)
            .GenerateNoiseMap(seed)
            .GenerateDisplayMap()
            .GeneratePathNodeMap()
            .PaintTileMap();

        var groundPathfinder = new GroundPathfinding();
        groundPathfinder.SetNodeMap(mapGenerator.PathNodeMap);
        unitController.SetGroundPathFinding(groundPathfinder);

        if (_debugger.TryGetComponent(out MainSceneDebugger debugger))
        {
            debugger.MapGenerator = mapGenerator;
            debugger.UnitController = unitController;
        }
    }
}
