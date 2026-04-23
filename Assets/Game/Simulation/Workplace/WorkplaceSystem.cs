using System;
using System.Collections.Generic;
using System.Linq;
using static Pop;
using static Workplace;
using static WorkplacesDefinitions;

public class WorkplaceSystem 
{
    private readonly IIntentBuffer _intents;

    List<IWorkplace> workplacesList;
    private int IdIncrement = 0;

    // list for workplace beeing build ?

    public WorkplaceSystem(IIntentBuffer intents)
    {
        workplacesList = new List<IWorkplace>();
        _intents = intents;
    }


    public void Initialize()
    {
        workplacesList = new List<IWorkplace>();
        
    }
     

    public void WorkplacesProduce()
    {
        
        foreach (var workplace in workplacesList)
        {
            if (workplace is IProductionBuilding production)
            {
                //Enqueu sell request
                //MarketSellResponse response = context.marketManager.MarketSell(production.SellRequest());
            }
        }
    }


   
 
    public void WorkplacesBuyInputGoods()
    {
        foreach (var workplace in workplacesList)
        {
            if (workplace is ITransformationBuilding transfoBuilding)
            {
                
            }
        }   
    }
    public void WorkplacePayEmployee()
    {
        //foreach (var workplace in workplacesList)
        //{
        //  List<IdNum> response = workplace.PayEmployees();

        //    if (response is not null)
        //    {

        //        context.populationManager.PayPop(response);
        //    }
        //    else

        //}

    }


    public void WorkplacePayOwner()
    {
        foreach (var workplace in workplacesList)
        {
            List<IdNum> response = workplace.PayOwners();
            if(response is not null)
            {
                //Enqueu request
            }
               
        }
    }
    public void WorkplaceHire(int workplaceId, int popId, int nummberOfHired, PopJob type)
    {
        IWorkplace workplace = workplacesList.Where(w => workplaceId == w.GetWorkplaceId()).FirstOrDefault();

        if (workplace == null)
        {
            throw new Exception($"Population manager outputed a non existant workplace Id : {workplaceId}");
        }
        workplace.OnWorkerHired(popId,nummberOfHired,type);
    }
    public void WorkplaceFire()
    {
         
    }
    
    public Workplace BuildWorkplace(DefinitionWorkplace type,int tileId,List<IdNum>_owner, DataRegistery _registry)
    {
        int tile = tileId;
        int provinceId = _registry.GetProvinceIdByTile(tile);
        int countryId = _registry.GetCountryIdByTile(tile);
        int id = IdIncrement++;
        Dictionary<int, int> constructionCost = type.constructionCost;
        Dictionary<int, int> maintenanceCost = type.maintenanceCost;
        Dictionary<int, int> workers = type.workers;

        List<GoodRequirement> _goodConstructionCost = new List<GoodRequirement>();
        List< GoodRequirement > _maintenanceCost = new List<GoodRequirement>();


        foreach (KeyValuePair<int, int> kvp in constructionCost) {
            GoodRequirement gr = new GoodRequirement (kvp.Key,0,kvp.Value);
            _goodConstructionCost.Add(gr);
        }
        foreach (KeyValuePair<int, int> kvp in maintenanceCost)
        {
            GoodRequirement gr = new GoodRequirement(kvp.Key, 0, kvp.Value);
            _maintenanceCost.Add(gr);
        }

        Workplace workplace = new Workplace(provinceId, id, countryId, _goodConstructionCost, _maintenanceCost,_owner,1,type.GetId());
        return workplace;
    }

    

    public List<IWorkplace> GetWorkplaceByProvinceId(int id)
    {
        return workplacesList.Where(w => w.GetProvinceId() == id).ToList();
    }

    public void OnDaily()
    {
        WorkplacesProduce();
        WorkplacePayEmployee();
    }

    public void OnMonth()
    {

    }

    //TODO  all workplace will be decleared in a json
    //public void CreateBasicRGOWorkplace(int provinceId, int countryID)
    //{
    //    //TODO All those stats should be defined in a json

    //    List<GoodRequirement> constructionCost = new List<GoodRequirement>();
    //    List<GoodRequirement> maintenanceCost = new List<GoodRequirement>();
    //    List<IdNum> owner = new List<IdNum>();
    //    List<WorkerTypeCurrentMax> workerTypeCurrentMaxs = new List<WorkerTypeCurrentMax>();
    //    int basicEfficiency = 1;
    //    int cashbufferMax = 100000;

    //    Workplace workplace = new Workplace(provinceId,IdIncrement,countryID,constructionCost,maintenanceCost, owner,100);
    //    Production production = new Production(cashbufferMax, basicEfficiency);

    //    //get province Rgo
    //    //List<Good> listGood = context.marketManager.GetGoodDefinition();

    //    ResourceGatheringOperation RGOworkplace = new ResourceGatheringOperation(workplace,production, listGood[0].id);

    //   IdIncrement++;
    //}
}

// Every province beggin with at least one ResourceGatheringOperation workplace own by all nobel pop of the province