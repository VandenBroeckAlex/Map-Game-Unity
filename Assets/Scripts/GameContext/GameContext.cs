using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Market_object;


public class GameContext : MonoBehaviour
{
    public static GameContext instance;
    public CountriesManager countriesManager;
    public ProvincesManager provincesManager;
    public PopulationManager populationManager;
    public MarketManager marketManager;
    public WorkplaceManager workplaceManager;
    public UI_Time_manager UiTimeManager;
    public UI_market_manager UI_Market_Manager;

    public GameContext() 
    {
        countriesManager = new CountriesManager(this);
        provincesManager = new ProvincesManager(this);
        populationManager = new PopulationManager(this);
        marketManager = new MarketManager(this);
        workplaceManager = new WorkplaceManager(this);
    }

    private void Awake()
    {
        CreateSingleton();
        
        // Create managers here
        countriesManager = new CountriesManager(this);
        provincesManager = new ProvincesManager(this);
        populationManager = new PopulationManager(this);
        marketManager = new MarketManager(this);
        workplaceManager = new WorkplaceManager(this);
    }
    public void Initialize()
    {
        GoodDatabase.Initialize();
        countriesManager.Initialize();
        provincesManager.Initialize();
        populationManager.InitializePopulation();
        marketManager.Initialize();

        OnTick();
        OnMonth();
    }


    //Game context should listen to tick and call everything in order
    public void OnTick()
    {
        // Avoid problem if called twice
        TickScript.onTick -= populationManager.PopListBuy;
        TickScript.onTick += populationManager.PopListBuy;

        TickScript.onTick -= populationManager.PopSearchWork;
        TickScript.onTick += populationManager.PopSearchWork;

        TickScript.onTick -= workplaceManager.OnTick;
        TickScript.onTick += workplaceManager.OnTick;

    }


    public void OnMonth()
    {
        // Avoid problem if called twice
        DateHandeler.onMonth -= populationManager.PopGrowth;
        DateHandeler.onMonth -= populationManager.ResetPopStockpile;
        DateHandeler.onMonth -= marketManager.PriceFluctuation;

        DateHandeler.onMonth += populationManager.PopGrowth;
        DateHandeler.onMonth += populationManager.ResetPopStockpile;
        DateHandeler.onMonth += marketManager.PriceFluctuation;
    }

    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("An instance of market manager already exist");
            Destroy(gameObject);
        }
    }
}
