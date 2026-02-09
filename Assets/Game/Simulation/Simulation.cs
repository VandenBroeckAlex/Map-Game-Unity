using System;

public class Simulation
{
    //System
    PopulationSystem popSystem = new PopulationSystem();
    MarketGood marketGood = new MarketGood();
    MarketSystem marketSystem = new MarketSystem();

    CountriesManager countriesManager = new CountriesManager();
    ProvincesSystem provincesSystem = new ProvincesSystem();

    //EventBus
    public static event Action<int, int> OnPopulationChanged;
}