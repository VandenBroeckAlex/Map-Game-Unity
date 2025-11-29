using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;

public class GameSceneInitiator : MonoBehaviour
{

    [SerializeField] private TickScript _tickScript;
    [SerializeField] private DateHandeler _dateHandeler;

    [SerializeField] private CountriesManager _countriesManager;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ProvincesManager _provincesManager;
    [SerializeField] private PopulationManager _populationManager;
    [SerializeField] private MarketManager _marketManager;
    [SerializeField] private Goods_loader _goods_loader;

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
        _tickScript.StartTickScript();


        Destroy(gameObject);
        
    }

    private void BindObject()
    {
        _root = new GameObject("GameRoot");
        _ui = new GameObject("Ui");

        _tickScript = Instantiate(_tickScript, _root.transform);
        _countriesManager = Instantiate(_countriesManager, _root.transform);
        _mainCamera = Instantiate(_mainCamera, _root.transform);
        _provincesManager = Instantiate(_provincesManager, _root.transform);
        _goods_loader = Instantiate(_goods_loader, _root.transform);
        _marketManager = Instantiate(_marketManager, _root.transform);
        _mapLoader = Instantiate(_mapLoader, _root.transform);
        _eventSystem = Instantiate(_eventSystem, _root.transform);
        _populationManager = Instantiate(_populationManager, _root.transform);

        _UiTimeManager = Instantiate(_UiTimeManager, _ui.transform);
        // optional others
       // _uI_Market_Manager  = Instantiate(_uI_Market_Manager, _root.transform);
        //_edgeGraphData = Instantiate(_edgeGraphData, _root.transform);
        //_uiDoc = Instantiate(_uiDoc, _root.transform);
        //_editProvUI = Instantiate(_editProvUI, _root.transform);
    }

    private async UniTask InitializeObject()
    {
          _tickScript.Initialize();
        Debug.Log("tickScript have been initialize");
          await UniTask.Yield();
         _countriesManager.Initialize();
        Debug.Log("countriesManager have been initialize");
        await UniTask.Yield();
        _provincesManager.Initialize();
        Debug.Log("provincesManager have been initialize");
        await UniTask.Yield();
        _marketManager.Initialize();
        Debug.Log("marketManager have been initialize");
        await UniTask.Yield();
        _populationManager.InitializePopulation();
        await UniTask.Yield();
        _UI_Market_Manager.CacheReferences();
        await UniTask.Yield();
        _UiTimeManager.Initialize();
    }
    //private async UniTask CreateObject()
    //{

    //}
}
