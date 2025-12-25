using System.Collections.Generic;
using static Market_object;
using static Workplace;


public class BuildingManager 
{
    private GameContext context;
    List<IWorkspace> workplaces;
    // list for workplace beeing build ?

    public BuildingManager(GameContext context)
    {
        this.context = context;
    }


    public void Initialize()
    {
        workplaces = new List<IWorkspace>();
    }
     

    public void WorkplaceProduce()
    {
        
        foreach (var workplace in workplaces)
        {
            if (workplace is IProductionBuilding production)
            {
                MarketSellResponse response = context.marketManager.MarketSell(production.SellRequest());
               
                workplace.ReciveCash(response.cashRecived);
            }
        }
        //
    }


   
 
    public void WorkplaceBuyInputGoods()
    {
        foreach (var workplace in workplaces)
        {
            if (workplace is ITransformationBuilding transfoBuilding)
            {

            }
        }
            
    }
    public void WorkplacePayEmployee()
    {
        foreach (var workplace in workplaces)
        {
          List<IdNum> response = workplace.PayEmployees();
            if (response is not null) 
            {
                context.populationManager.PayPop(response);
            }          
        }
    }


    public void WorkplacePayOwner()
    {
        foreach (var workplace in workplaces)
        {
            List<IdNum> response = workplace.PayOwners();
            if(response is not null)
                context.populationManager.PayPop(response);
        }
    }
    public void WorkplaceHire()
    {

    }
    public void WorkplaceFire()
    {

    }
    public void BuildWorkplace()
    {

    }


    public void OnTick()
    {

    }

    public void OnDaily()
    {

    }

    public void OnMonth()
    {

    }
}

// Every province beggin with at least one ResourceGatheringOperation workplace own by all nobel pop of the province