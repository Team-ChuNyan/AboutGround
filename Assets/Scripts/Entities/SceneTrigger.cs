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
        // 매니저 생성
        new DataManager().InitializeItemData();
        new ItemGenerator();
        var packGenerator = gameObject.AddComponent<PackGenerator>();
        var unitGenerator = gameObject.AddComponent<UnitGenerator>();
        var workGenerator = new WorkGenerator();

        // 클래스 생성
        var inputController = gameObject.AddComponent<PlayerInputController>();
        var unitController = gameObject.AddComponent<UnitController>();
        var itemController = gameObject.AddComponent<ItemController>();

        var mapGenerator = Instantiate(Resources.Load<MapGenerator>("Prefabs/MapGenerator"));
        mapGenerator.name = "MapGenerator";
        var GroundGiud = Instantiate(Resources.Load<TilePainter>("Prefabs/GroundGrid"));
        GroundGiud.name = "GrundGrid";

        var workplan = new WorkPlan();
        var groundPathfinder = new GroundPathfinding();

        // 클래스 초기화
        SeedMapData seed = new(100, 100, 9123);
        mapGenerator.Initialize(GroundGiud)
            .GenerateNoiseMap(seed)
            .GenerateDisplayMap()
            .GeneratePathNodeMap()
            .PaintTileMap();

        unitController.SetGroundPathFinding(groundPathfinder);
        unitController.Initialize(workplan);
        workGenerator.Initialize(workplan);

        groundPathfinder.SetNodeMap(mapGenerator.PathNodeMap);

        // 디버거
        if (_debugger.TryGetComponent(out MainSceneDebugger debugger))
        {
            debugger.MapGenerator = mapGenerator;
            debugger.UnitController = unitController;
        }
    }
}
