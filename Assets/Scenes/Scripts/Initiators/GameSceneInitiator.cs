using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

public class GameSceneInitiator : MonoBehaviour
{

    [SerializeField] private DateHandeler _dateHandeler;
    private GameContext _gameContext = new GameContext();    
 
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private TickScript _tickScript;
    [SerializeField] private MapLoader _mapLoader;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private EdgeGraphData _edgeGraphData;
    //[SerializeField] private UI_market_manager _uI_Market_Manager;
    [SerializeField] private UI_Time_manager _UiTimeManager;
    [SerializeField] private UI_market_manager _UI_Market_Manager;

    //[SerializeField] private ProvinceUIController _editProvUI;

    private GameObject _root;
    private GameObject _ui;
    private async void Start()
    {
        BindObject();
        await InitializeObject();
        //await CreateObject();



        Destroy(gameObject);
        
    }

    private void BindObject()
    {
        _root = new GameObject("GameRoot");
        _ui = new GameObject("Ui");
        _tickScript = Instantiate(_tickScript, _root.transform);
        _mainCamera = Instantiate(_mainCamera, _root.transform);
        _mapLoader = Instantiate(_mapLoader, _root.transform);
        _eventSystem = Instantiate(_eventSystem, _root.transform);
        _UiTimeManager = Instantiate(_UiTimeManager, _ui.transform);
        // optional others
       // _uI_Market_Manager  = Instantiate(_uI_Market_Manager, _root.transform);
        //_edgeGraphData = Instantiate(_edgeGraphData, _root.transform);
        //_uiDoc = Instantiate(_uiDoc, _root.transform);
        //_editProvUI = Instantiate(_editProvUI, _root.transform);
    }

    private async UniTask InitializeObject()
    {
        _gameContext.Initialize();
        Debug.Log("GameContext have been initialized");
          await UniTask.Yield();
        _tickScript.Initialize();
        _UiTimeManager.Initialize();
    }
   
}
