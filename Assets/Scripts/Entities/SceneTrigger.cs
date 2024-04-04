using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneType _currentScene;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private GameObject _debugger;

    [SerializeField] private Vector3 _startCameraPosition;
    [SerializeField] private Vector3 _startCameraRotation;

    private ItemGenerator _itemGenerator;
    private PackGenerator _packGenerator;
    private UnitGenerator _unitGenerator;
    private WorkGenerator _workGenerator;
    private MapGenerator _mapGenerator;

    private CameraController _cameraController;
    private VirtualCameraController _virualCameraController;
    private PlayerInputController _inputController;
    private UnitController _unitController;
    private ItemController _itemController;
    private SelectPropController _selectController;
    private InGameUIController _inGameUIController;

    private TilePainter _tilepainter;
    private WorkPlan _workplan;
    private GroundPathfinding _groundPathfinder;
    private QuickCanceling _quickCanceling;
    private PropsContainer _propsContainer;

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
        GetClassData();
        
        InitPropsContainer();
        CreateMap();
        InitClass();
        InitDebuger();

        RegisterInteractionViewModel();
    }

    private void InstantiateManagers()
    {
        new DataManager().InitializeItemData();
        _itemGenerator = new ItemGenerator();
        _packGenerator = gameObject.AddComponent<PackGenerator>();
        _unitGenerator = gameObject.AddComponent<UnitGenerator>();
        _workGenerator = new WorkGenerator();
    }

    private void InstantiateController()
    {
        _virualCameraController = _virtualCamera.AddComponent<VirtualCameraController>();
        _inputController = gameObject.AddComponent<PlayerInputController>();
        _unitController = gameObject.AddComponent<UnitController>();
        _itemController = gameObject.AddComponent<ItemController>();
        _selectController = new SelectPropController();

        _workplan = new WorkPlan();
        _groundPathfinder = new GroundPathfinding();
        _quickCanceling = new QuickCanceling();
        _propsContainer = new PropsContainer();
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

    private void InitPropsContainer()
    {
        _propsContainer.SetPacks(PackGenerator.Instance.ActivePack)
            .SetPlayerUnits(_unitController.PlayerUnits)
            .SetNpcUnits(_unitController.NPCUnits);
    }

    private void InitClass()
    {
        _cameraController.SetCameraPosition(_startCameraPosition);
        _cameraController.SetCameraRotation(_startCameraRotation);

        _unitGenerator.Init(_groundPathfinder);
        _workGenerator.Initialize(_workplan);

        _cameraController.Initialize(_inputController, _virualCameraController);
        _unitController.Init(_workplan);
        _groundPathfinder.SetNodeMap(_mapGenerator.PathNodeMap);
        _selectController.Init(_inputController, _quickCanceling);
        _selectController.InitObjectSelecting(_interactionViewModel, _inGameUIController.DragSelectionUI, _propsContainer);

        _quickCanceling.Init(_inputController);
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

    private void InitDebuger()
    {
        if (_debugger.TryGetComponent(out MainSceneDebugger debugger))
        {
            debugger.MapGenerator = _mapGenerator;
            debugger.UnitController = _unitController;
        }

        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                UnitGenerator.Instance.SetNewUnit(PropOwner.Player, RaceType.Human)
                    .SetPosition(new Vector3(x, 0, z));

                var item = ItemGenerator.Instance.SetNewItem(ItemType.Apple)
                    .GetNewItem();
                PackGenerator.Instance.CreateNewItemPack(item).SetPosition(new Vector2Int(x + 50, z + 50));
            }
        }
    }

    private void RegisterInteractionViewModel()
    {
        _interactionViewModel.RegisterEvent(InteractionType.Cancel, _selectController.QuickCancel);
        _interactionViewModel.RegisterEvent(InteractionType.Carry, () => { PackController.CarryAll(_interactionViewModel.Selections); });
    }
}
