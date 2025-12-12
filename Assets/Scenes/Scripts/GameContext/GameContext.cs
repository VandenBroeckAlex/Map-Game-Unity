using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameContext : MonoBehaviour
{

    public CountriesManager countriesManager;
    public ProvincesManager provincesManager;
    public PopulationManager populationManager;
    public MarketManager marketManager;
    public UI_Time_manager UiTimeManager;
    public UI_market_manager UI_Market_Manager;

    public GameContext() 
    {
        
        countriesManager = new CountriesManager(this);
        provincesManager = new ProvincesManager(this);
        populationManager = new PopulationManager(this);
        marketManager = new MarketManager(this);
    }




    
    public void Initialize()
    {
        GoodDatabase.Initialize();
        Debug.Log($"Market Good count : {GoodDatabase.good_definition_list.Count}");
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
        TickScript.onTick += populationManager.PopBuy;
        TickScript.onTick += populationManager.PopSell;
    }


    public void OnMonth()
    {
        DateHandeler.onMonth += populationManager.PopGrowth;
        DateHandeler.onMonth += populationManager.ResetPopStockpile;
        DateHandeler.onMonth += marketManager.PriceFluctuation;
    }
}
