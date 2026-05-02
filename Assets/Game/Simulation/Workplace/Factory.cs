using System.Collections.Generic;
using static WorkplacesDefinitions;
using static MarketTransactionsObj;
public class Factory : IWorkplace,IProductionBuilding, ITransformationBuilding
{
    public Workplace workplace;
    public Production production;
    List<GoodRequirement> inputGoods;
    public Dictionary<string, int> outputGoods;

    //Type of production
    //Type of remuneration 

    public Factory(
        Production _productionWorkplace,
        Dictionary<string, int> _inputGoods,
        Dictionary<string, int> _outputGoods
        )
    {
        production = _productionWorkplace;
        inputGoods = _inputGoods;
        outputGoods = _outputGoods;
    }

    public void AddCash(int cash)
    {
        production.cashBuffer += cash;
        // if cashBuffer + cash > bufferMax => put money in bank
    }

    public void AddGood(int id, int amount)
    {
        // Try to fill maintenance goods first
        amount = UpdateStockpileInList(workplace.maintenanceGoods, id, amount);

        // If there is still amount left, try to fill input goods
        if (amount > 0)
        {
            UpdateStockpileInList(inputGoods, id, amount);
        }
    }

    public MarketBuyRequest BuyMaintenanceGood()
    {
        MarketBuyRequest request = new MarketBuyRequest();
        request.id = workplace.id;
        request.marketId = workplace.marketId;
        request.cashAmount = production.cashBuffer;
        request.GoodRequests = new List<GoodRequest>();
        foreach (GoodRequirement good in workplace.maintenanceGoods)
        {
            int goodId = good.good_id;
            int ammount = good.maxNeed - good.stockpile;
            GoodRequest gr = new GoodRequest(goodId,ammount);
            request.GoodRequests.Add(gr);
        } 
        return request;
    }

    public void Degrade()
    {
        throw new System.NotImplementedException();
    }

    public int GetProvinceId()
    {
        return workplace.GetProvinceId();
    }

    public int GetWorkAvailableByJobType(PopJob popJob)
    {
        return workplace.GetWorkAvailableByJobType(popJob);
    }

    public int GetWorkplaceId()
    {
        return workplace.id;
    }

    public List<IdNum> LayOffWorker()
    {
        throw new System.NotImplementedException();
    }

    public void OnWorkerHired(int popId, int numberOfHired, PopJob job)
    {
        workplace.HireWorker(popId, numberOfHired, job);
    }

    public void OnWorkerLeave(int popId, int numberOfLeaving)
    {
        workplace.LayOffWorkerByID(popId, numberOfLeaving);
    }

    public List<IdNum> PayEmployees()
    {
        return workplace.PayEmployees();
    }

    public List<IdNum> PayOwners()
    {
        throw new System.NotImplementedException();
    }



 

    public void SetCashTo(int ammount)
    {
        production.cashBuffer = ammount;
    }

    public void SetWages()
    {
        throw new System.NotImplementedException();
    }

    public void TakeLoan()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public MarketSellRequest Produce()
    {
        int baseOutput = production.baseOutput;
        int workerRatio = workplace.SmallestProducerWorkerRatio();
        //input good ratio
        //Min output - min workforce
        int ouput = 0;
    }

    //---- Private ------

    //TODO
    private bool CanProduce(Dictionary<string, int> goodsStockpile, int workerCount)
    {

        //foreach (var input in inputGoods)
        //    if (!goodsStockpile.ContainsKey(input.Key) || goodsStockpile[input.Key] < input.Value)
        //        return false;
        return true;
    }


    private int UpdateStockpileInList(IEnumerable<GoodRequirement> list, int id, int amount)
    {
        foreach (var gr in list)
        {
            if (gr.good_id == id)
            {
                int total = gr.stockpile + amount;

                if (total <= gr.maxNeed)
                {
                    gr.stockpile = total;
                    return 0; // Everything fits, nothing left to carry over
                }
                else
                {
                    gr.stockpile = gr.maxNeed;
                    return total - gr.maxNeed; // Return the overflow amount
                }
            }
        }
        return amount; // ID wasn't found in this list, return original amount
    }

    public MarketBuyRequest BuyInputGoods()
    {
        throw new System.NotImplementedException();
    }
}


