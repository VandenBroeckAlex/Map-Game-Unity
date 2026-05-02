using System;
using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;
using static Pop;
using static Workplace;
using static WorkplacesDefinitions;
public class WorkplaceSystem
{
    private readonly IIntentBuffer _intents;

    List<IWorkplace> workplacesList;
    private int IdIncrement = 0;

    // list for workplace beeing build ?

    public WorkplaceSystem(IIntentBuffer intents, List<IWorkplace> workplacesList)
    {
        this.workplacesList = workplacesList;
        _intents = intents;
    }


    public void Initialize()
    {
        workplacesList = new List<IWorkplace>();

    }


    public List<MarketSellRequest> WorkplacesProduce(DataRegistery _registery)
    {
        List<MarketSellRequest> requests = new List<MarketSellRequest>();
        foreach (var workplace in workplacesList)
        {
            if (workplace is IProductionBuilding production)
            {
                requests.Add(production.Produce());
            }
        }
        return requests;
    }
    
    public void WorkplacesBuyGoods()
    {
        List<MarketBuyRequest> requests = new List<MarketBuyRequest>();
        foreach (var workplace in workplacesList)
        {
            MarketBuyRequest request = workplace.BuyMaintenanceGood();
            if (workplace is ITransformationBuilding transfoBuilding)
            {
                MarketBuyRequest input = transfoBuilding.BuyInputGoods();

                request.GoodRequests.Concat(input.GoodRequests);
            }
            requests.Add(request);
        }
    }
    public List<IdNum> WorkplacePayEmployee(DataRegistery _registery)
    {
        List<IdNum> data = new List<IdNum>();
        foreach (Workplace workplace in workplacesList)
        {
            List<IdNum> response = workplace.PayEmployees();

            if (response is not null)
            {
                data.Concat(response);
            }
        }
        return data;

    }
    public void WorkplacePayOwner()
    {
        foreach (var workplace in workplacesList)
        {
            List<IdNum> response = workplace.PayOwners();
            if (response is not null)
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
        workplace.OnWorkerHired(popId, nummberOfHired, type);
    }
    public void WorkplaceFire()
    {
        throw new System.NotImplementedException();
    }
    public void WorkplaceProcessMarketBuyResponse(List<MarketBuyResponse> responses)
    {
        foreach (MarketBuyResponse response in responses)
        {
            IWorkplace workplace = GetWorkplaceById(response.id);

            foreach (GoodRequest good in response.goodsBought)
            {
                workplace.AddGood(good.goodId, good.amount);
            }
            workplace.SetCashTo(response.cashLeft);
        }
    }
    public void WorkplaceProcessMarketSellResponse(List<MarketSellResponse> responses)
    {
        foreach (MarketSellResponse response in responses)
        {
            IWorkplace workplace = GetWorkplaceById(response.id);
            workplace.AddCash(response.cashRecived);
        }
    }
    public Workplace BuildWorkplace(DefinitionWorkplace type, int tileId, List<IdNum> _owner, DataRegistery _registry)
    {
        int tile = tileId;
        int provinceId = _registry.GetProvinceIdByTile(tile);
        int countryId = _registry.GetCountryIdByTile(tile);
        int id = IdIncrement++;
        Dictionary<int, int> constructionCost = type.constructionCost;
        Dictionary<int, int> maintenanceCost = type.maintenanceCost;
        Dictionary<int, int> workers = type.workers;

        List<GoodRequirement> _goodConstructionCost = new List<GoodRequirement>();
        List<GoodRequirement> _maintenanceCost = new List<GoodRequirement>();


        foreach (KeyValuePair<int, int> kvp in constructionCost)
        {
            GoodRequirement gr = new GoodRequirement(kvp.Key, 0, kvp.Value);
            _goodConstructionCost.Add(gr);
        }
        foreach (KeyValuePair<int, int> kvp in maintenanceCost)
        {
            GoodRequirement gr = new GoodRequirement(kvp.Key, 0, kvp.Value);
            _maintenanceCost.Add(gr);
        }

        Workplace workplace = new Workplace(provinceId, id, countryId, _goodConstructionCost, _maintenanceCost, _owner, 1, type.GetId());
        return workplace;
    }



    public List<IWorkplace> GetWorkplaceByProvinceId(int id)
    {
        return workplacesList.Where(w => w.GetProvinceId() == id).ToList();
    }

    public IWorkplace GetWorkplaceById(int id)
    {
        return workplacesList.Where(w => w.GetWorkplaceId() == id).FirstOrDefault();
    }

    public void DestroyBuilding(int id)
    {
        IWorkplace workplace = GetWorkplaceById(id);
        workplacesList.Remove(workplace);
    }

    public void OnDaily(DataRegistery _registery)
    {
        WorkplacesProduce(_registery);
        WorkplacePayEmployee(_registery);
    }

    public void OnMonth()
    {

    }
}