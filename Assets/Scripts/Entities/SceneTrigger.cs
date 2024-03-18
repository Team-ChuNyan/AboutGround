using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneType _currentScene;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private GameObject _debugger;

    [SerializeField] private Vector3 _startCameraPosition;
    [SerializeField] private Vector3 _startCameraRotation;

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
        var cameraInputHandler = InstantiateCameraSystem();
        var virualcameraController = _virtualCamera.AddComponent<VirtualCameraController>();
        var inputController = gameObject.AddComponent<PlayerInputController>();
        var unitController = gameObject.AddComponent<UnitController>();
        var itemController = gameObject.AddComponent<ItemController>();

        var inGameUI = Instantiate(Resources.Load<InGameUIController>("Prefabs/InGameUI"));
        inGameUI.name = "InGameUI";
        var interActionMenuUIViewModel = inGameUI.GetInteractionMenuViewModel();

        var mapGenerator = Instantiate(Resources.Load<MapGenerator>("Prefabs/MapGenerator"));
        mapGenerator.name = "MapGenerator";
        var GroundGiud = Instantiate(Resources.Load<TilePainter>("Prefabs/GroundGrid"));
        GroundGiud.name = "GrundGrid";

        var workplan = new WorkPlan();
        var groundPathfinder = new GroundPathfinding();
        var ObjectSelector = new ObjectSelector();

        // 클래스 초기화
        cameraInputHandler.Initialize(inputController, virualcameraController);

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
        ObjectSelector.Init(inputController, inGameUI.DragSelectionUI);
        ObjectSelector.SetTargetProp(unitController.PlayerUnit);

        // 디버거
        if (_debugger.TryGetComponent(out MainSceneDebugger debugger))
        {
            debugger.MapGenerator = mapGenerator;
            debugger.UnitController = unitController;
        }

        for (int x = 0; x < 70; x++)
        {
            for (int z = 0; z < 70; z++)
            {
                unitController.CreateNewPlayerUnit(RaceType.Human).transform.position = new Vector3(x, 0, z);
            }
        }
    }

    private CameraController InstantiateCameraSystem()
    {
        var cameraSystem = Instantiate(Resources.Load<CameraController>("Prefabs/CameraSystem"));
        cameraSystem.name = "CameraSystem";
        cameraSystem.SetCameraPosition(_startCameraPosition);
        cameraSystem.SetCameraRotation(_startCameraRotation);

        return cameraSystem;
    }
}
