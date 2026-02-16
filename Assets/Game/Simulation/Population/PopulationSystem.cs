using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design;
using static MarketTransactionsObj;

public class PopulationSystem
{
    HandleGet getter = new HandleGet();


    EventBinding<PopBuyRequest> testEventBinding;

    private readonly List<Pop> _popList;
    public PopulationSystem(List<Pop> popList, EventBinding<PopBuyRequest> testEventBinding)
    {
        _popList = popList;
        this.testEventBinding = testEventBinding;
    }

    void Tick()
    {
        HandlePopBuy.PopulationBuyRequest(_popList);
    }

    
    
}