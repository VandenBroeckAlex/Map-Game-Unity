using System;

public class Simulation
{
   
     
    //System
    private readonly PopulationSystem _populationSystem;
    private readonly MarketGood _marketGood;
    private readonly MarketSystem _marketSystem;
    private readonly CountriesManager _countriesManager;
    private readonly ProvincesSystem _provincesSystem;
    private readonly IntentBuffer _commandBus = new IntentBuffer();
    //EventBus
    //public static event Action<int, int> OnPopulationChanged;

    public Simulation( MarketSystem marketSystem,CountriesManager countriesManager, ProvincesSystem provincesSystem)
    {
        //_populationSystem = new PopulationSystem(_intentBuffer);
        _marketSystem = marketSystem;
        _countriesManager = countriesManager;
        _provincesSystem = provincesSystem;

    }

}