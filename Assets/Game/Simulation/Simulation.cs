using System;

public class Simulation
{
   
     
    //System
    private readonly PopulationSystem _populationSystem;
    private readonly MarketGood _marketGood;
    private readonly CountriesManager _countriesManager;
    private readonly ProvincesSystem _provincesSystem;
    private readonly IntentBuffer _commandBus = new IntentBuffer();
    //EventBus
    //public static event Action<int, int> OnPopulationChanged;

    public Simulation( CountriesManager countriesManager, ProvincesSystem provincesSystem)
    {
        //_populationSystem = new PopulationSystem(_intentBuffer);
        _countriesManager = countriesManager;
        _provincesSystem = provincesSystem;

    }

}