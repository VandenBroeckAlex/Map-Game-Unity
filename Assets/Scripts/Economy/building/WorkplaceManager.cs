using System;
using System.Collections.Generic;
using System.Linq;
using static Market_object;
using static Pop_objects;
using static Workplace;
using UnityEngine;
using Unity.Jobs.LowLevel.Unsafe;

public class WorkplaceManager 
{
    private GameContext context;
    List<IWorkplace> workplacesList;
    private int IdIncrement = 0;

    // list for workplace beeing build ?

    public WorkplaceManager(GameContext context)
    {
        this.context = context;
        workplacesList = new List<IWorkplace>();
    }


    public void Initialize()
    {
        workplacesList = new List<IWorkplace>();
        
    }
     

    public void WorkplaceProduce()
    {
        
        foreach (var workplace in workplacesList)
        {
            if (workplace is IProductionBuilding production)
            {
                MarketSellResponse response = context.marketManager.MarketSell(production.SellRequest());
               
                workplace.ReciveCash(response.cashRecived);
            }
        }
    }


   
 
    public void WorkplaceBuyInputGoods()
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
        foreach (var workplace in workplacesList)
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
        foreach (var workplace in workplacesList)
        {
            List<IdNum> response = workplace.PayOwners();
            if(response is not null)
                context.populationManager.PayPop(response);
        }
    }
    public void WorkplaceHire(int workplaceId, int popId, int nummberOfHired, PopJob type)
    {
        IWorkplace workplace = workplacesList.Where(w => workplaceId == w.GetWorkplaceId()).FirstOrDefault();

        if (workplace == null)
        {
            throw new Exception("Population manager outputed a non existant workplace Id : BuildingManager.WorkplaceHire()");
        }
        workplace.OnWorkerHired(popId,nummberOfHired,type);
    }
    public void WorkplaceFire()
    {
         
    }
    
    public void BuildWorkplace(string type)
    {

    }

    public void CreateGrainFarm(int provinceId)
    {
        Debug.Log($"Create Grain Farm in province {provinceId}");
        int countryId = context.provincesManager.GetProvinceOwnerByProvinceId(provinceId);

        List<GoodRequirement> goodRequirements = new List<GoodRequirement>();

        GoodRequirement req = new GoodRequirement(1,0 ,10000);

        goodRequirements.Add(req);

        Workplace workplace = new Workplace(provinceId, IdIncrement, countryId, goodRequirements, goodRequirements, new List<IdNum>(),1);
        
        Production prod = new Production(100,100);
        
        ResourceGatheringOperation farm = new ResourceGatheringOperation(workplace,prod,_outputGoodId : 5);


        WorkerTypeCurrentMax workerType = new WorkerTypeCurrentMax(PopulationManager.jobTypes[0]);

        workplace.workersRequirement.Add(workerType);

        workplacesList.Add(farm);
        IdIncrement++;
    }


    public List<IWorkplace> GetWorkplaceByProvinceId(int id)
    {
        return workplacesList.Where(w => w.GetProvinceId() == id).ToList();
    }


    public void OnTick()
    {

    }

    public void OnDaily()
    {
        WorkplaceProduce();
        WorkplacePayEmployee();
    }

    public void OnMonth()
    {

    }

    //TODO  all workplace will be decleared in a json
    public void CreateBasicRGOWorkplace(int provinceId, int countryID)
    {
        //TODO All those stats should be defined in a json

        List<GoodRequirement> constructionCost = new List<GoodRequirement>();
        List<GoodRequirement> maintenanceCost = new List<GoodRequirement>();
        List<IdNum> owner = new List<IdNum>();
        List<WorkerTypeCurrentMax> workerTypeCurrentMaxs = new List<WorkerTypeCurrentMax>();
        int basicEfficiency = 1;
        int cashbufferMax = 100000;

        Workplace workplace = new Workplace(provinceId,IdIncrement,countryID,constructionCost,maintenanceCost, owner,100);
        Production production = new Production(cashbufferMax, basicEfficiency);

        //get province Rgo
        List<Good> listGood = context.marketManager.GetGoodDefinition();

        ResourceGatheringOperation RGOworkplace = new ResourceGatheringOperation(workplace,production, listGood[0].id);

       IdIncrement++;
    }
}

// Every province beggin with at least one ResourceGatheringOperation workplace own by all nobel pop of the province