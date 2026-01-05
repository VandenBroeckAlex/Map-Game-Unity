using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Cysharp.Threading.Tasks;
public class MapEditorInitiator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private CountriesManager _countriesManager;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private ProvincesManager _provincesManager;
    [SerializeField] private GameContext _gameContext;

    [SerializeField] private MapLoader _mapLoader;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private PopulationManager _populationManager;
    [SerializeField] private EdgeGraphData _edgeGraphData;
    [SerializeField] private SpriteCreator_v5 _spriteCreator;

    [SerializeField] private UIDocument _uiDoc;
    [SerializeField] private ProvinceUIController _editProvUI;

    private async void Start()
    {
        BindObject();
        await InitializeObject();
        //await CreateObject();
        Destroy(gameObject);
    }

    private void BindObject()
    {
        _gameContext = Instantiate(_gameContext); // !
        _mainCamera = Instantiate(_mainCamera);
        _mapLoader = Instantiate(_mapLoader);
        _eventSystem = Instantiate(_eventSystem);
        _edgeGraphData = Instantiate(_edgeGraphData);
        _spriteCreator = Instantiate(_spriteCreator);
        _uiDoc = Instantiate(_uiDoc);
        _editProvUI = Instantiate(_editProvUI);
        
    }

    private async UniTask InitializeObject()
    {
        _countriesManager.Initialize();
        await UniTask.Yield();
        _provincesManager.Initialize();
        await UniTask.Yield();
    }
    //private async UniTask CreateObject()
    //{

    //}
}
