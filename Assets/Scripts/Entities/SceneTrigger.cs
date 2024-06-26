using AboutGround.GroundMap;
using AboutGround.GroundMap.Generator;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneType _currentScene;
    [SerializeField] private GameObject _virtualCamera;

    [SerializeField] private Vector3 _startCameraPosition;
    [SerializeField] private Vector3 _startCameraRotation;

    private MapGenerator _mapGenerator;

    private CameraController _cameraController;
    private VirtualCameraController _virualCameraController;
    private MouseInputHandler _mouseInputHandler;
    private UnitController _unitController;
    private ItemController _itemController;
    private SelectPropController _selectController;
    private InGameUIController _inGameUIController;
    private PackController _packController;

    private TilePainter _tilepainter;
    private WorkPlan _workplan;
    private GroundPathfinding _groundPathfinder;
    private QuickCanceling _quickCanceling;
    private PropsContainer _propsContainer;
    private BlueprintConstructing _blueprintConstructing;

    private InteractionViewModel _interactionViewModel;

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
        InstantiateManagers();
        InstantiateController();
        InstantiateGameObject();
        InitUI_TempMethod();
        GetClassData();

        InitPropsContainer();
        CreateMap();
        InitClass();
        InitDebuger();

        RegisterInteractionViewModel();
        RegisterEvent();

        ChangeMouseModeDefault();
    }

    private void InstantiateManagers()
    {
        new DataManager();
        new WorkProcessGenerator();
        new ItemGenerator();

        gameObject.AddComponent<PlayerInputManager>();
        gameObject.AddComponent<PackGenerator>();
        gameObject.AddComponent<UnitGenerator>();
        gameObject.AddComponent<BuildingGenerator>();
    }

    private void InstantiateController()
    {
        _virualCameraController = _virtualCamera.AddComponent<VirtualCameraController>();
        _mouseInputHandler = new();
        _unitController = gameObject.AddComponent<UnitController>();
        _itemController = new();
        _selectController = new SelectPropController();
        _packController = new();

        _workplan = new WorkPlan();
        _groundPathfinder = new GroundPathfinding();
        _quickCanceling = new QuickCanceling();
        _propsContainer = new PropsContainer();
        _blueprintConstructing = new();
    }

    private void InstantiateGameObject()
    {
        _inGameUIController = Instantiate(Resources.Load<InGameUIController>("Prefabs/InGameUI"));
        _inGameUIController.name = "InGameUI";
        _mapGenerator = Instantiate(Resources.Load<MapGenerator>("Prefabs/MapGenerator"));
        _mapGenerator.name = "MapGenerator";
        _tilepainter = Instantiate(Resources.Load<TilePainter>("Prefabs/GroundGrid"));
        _tilepainter.name = "GrundGrid";
        _cameraController = Instantiate(Resources.Load<CameraController>("Prefabs/CameraSystem"));
        _cameraController.name = "CameraSystem";
    }

    private void GetClassData()
    {
        _interactionViewModel = _inGameUIController.InteractionMenuUI.ViewModel;
    }

    private void InitUI_TempMethod()
    {
        _inGameUIController.Init(_quickCanceling);
        // TODO : quickCanceling이 이벤트 등록하도록 변경
    }

    private void InitPropsContainer()
    {
        _propsContainer.SetPacks(_packController.Packs)
            .SetPlayerUnits(_unitController.PlayerUnits)
            .SetNpcUnits(_unitController.NPCUnits);
    }

    private void InitClass()
    {
        _cameraController.SetCameraPosition(_startCameraPosition);
        _cameraController.SetCameraRotation(_startCameraRotation);

        UnitGenerator.Instance.Init(_groundPathfinder);

        _cameraController.Initialize(_virualCameraController);
        _unitController.Init(_workplan);
        _groundPathfinder.SetNodeMap(_mapGenerator.Grounds);
        _selectController.Init(_quickCanceling);
        _selectController.InitObjectSelecting(_interactionViewModel, _inGameUIController.DragSelectionUI, _propsContainer, _mouseInputHandler);
        _itemController.Init();

        _quickCanceling.Init();
        _mouseInputHandler.Init();
        _blueprintConstructing.Init(_mouseInputHandler, _quickCanceling);
    }

    private void ChangeMouseModeDefault()
    {
        _mouseInputHandler.ChangeLeftClickMode(MouseInputHandler.LeftClick.Selecting);
    }

    private void CreateMap()
    {
        SeedMapData seed = new(100, 100, 9123);
        _mapGenerator.Initialize(_tilepainter)
            .GenerateNoiseMap(seed)
            .GenerateDisplayMap()
            .GeneratePathNodeMap()
            .PaintTileMap();
    }

    private void RegisterEvent()
    {
        _inGameUIController.BuildUI.RegisterItemClicked(_blueprintConstructing.Start);
        _inGameUIController.BuildUI.RegisterCanceled(_blueprintConstructing.Cancel);
    }

    private void InitDebuger()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                if (x == 0)
                {
                    UnitGenerator.Instance.Prepare(PropOwner.Player, RaceType.Human)
                                          .SetPosition(new Vector3(x, 0, z));
                }

                var item = ItemGenerator.Instance.Prepare(ItemType.Apple)
                                                 .Generate();
                PackGenerator.Instance.Prepare(item)
                                      .SetPosition(new Vector3(x + 50, 0, z + 50));
            }
        }

        BuildingGenerator.Instance.Prepare(BuildingType.Wall)
                                  .SetPosition(new Vector2Int(10, 50))
                                  .Generate();
    }

    private void RegisterInteractionViewModel()
    {
        _interactionViewModel.RegisterEvent(InteractionType.Cancel, _selectController.QuickCancel);
        _interactionViewModel.RegisterEvent(InteractionType.Carry, () => { PackController.CarryAll(_interactionViewModel.Selections); });
    }
}
