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
    [SerializeField] private UI_market_manager _uI_Market_Manager;

    [SerializeField] private UIDocument _uiDoc;
    //[SerializeField] private ProvinceUIController _editProvUI;

    private async void Start()
    {
        BindObject();
        await InitializeObject();
        await CreateObject();
        _tickScript.StartTickScript();


        Destroy(gameObject);
        
    }

    private void BindObject()
    {
        _tickScript = Instantiate(_tickScript);
        _countriesManager = Instantiate(_countriesManager);
        _mainCamera = Instantiate(_mainCamera);
        _provincesManager = Instantiate(_provincesManager);
        _goods_loader = Instantiate(_goods_loader);
        _marketManager = Instantiate(_marketManager);
        _mapLoader = Instantiate(_mapLoader);
        _eventSystem = Instantiate(_eventSystem);
        _populationManager = Instantiate(_populationManager);
        //_uI_Market_Manager  = Instantiate(_uI_Market_Manager);
        //_edgeGraphData = Instantiate(_edgeGraphData);
        //_uiDoc = Instantiate(_uiDoc);
        //_editProvUI = Instantiate(_editProvUI); 

    }

    private async UniTask InitializeObject()
    {
          _tickScript.Initialize();
         _countriesManager.Initialize();
         _provincesManager.Initialize();
         _marketManager.Initialize();
         _populationManager.InitializePopulation();
        _uI_Market_Manager.Initilize(); 
    }
    private async UniTask CreateObject()
    {

    }
}
