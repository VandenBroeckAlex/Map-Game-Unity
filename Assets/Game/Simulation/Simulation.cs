using System;

public class Simulation
{
    private readonly IntentBuffer _intentBuffer;
     
    //System
    private readonly PopulationSystem _populationSystem;
    private readonly MarketGood _marketGood;
    private readonly MarketSystem _marketSystem;
    private readonly CountriesManager _countriesManager;
    private readonly ProvincesSystem _provincesSystem;

    //EventBus
    //public static event Action<int, int> OnPopulationChanged;

    public Simulation()
    {
        _intentBuffer = new IntentBuffer();
        _populationSystem = new PopulationSystem(_intentBuffer);
        _marketSystem = new MarketSystem();
        _countriesManager = new CountriesManager();
        _provincesSystem = new ProvincesSystem();
    }

}