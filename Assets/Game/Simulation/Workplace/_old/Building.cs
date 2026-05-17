using System.Collections.Generic;
using static MarketTransactionsObj;

public class Building
{
    string name;
    int id;
    //int tileId;
    int provinceId;
    int countryId;
    int marketId;
    int size;
    public Workplace workplace;
    public Production production;
 
    public Dictionary<string, int> outputGoods;
    
    public bool canProduce => production != null;
    public bool isRGO;
    public bool isCrop;

    public int GetProvinceId()
    {
        return provinceId;
    }
    public int GetWorkplaceId()
    {
        return id;
    }

    public int GetCountryId()
    {
        return countryId;
    }
    public int GetMarketId()
    {
        return marketId;
    }
    public void AddCash(int cash)
    {
        if (production != null)
        {
            production.cashBuffer += cash;
            //TODO if cashBuffer + cash > bufferMax => put money in bank
        }
    }
    public void SetCash(int ammount)
    {
        if (production != null)
        {
            production.cashBuffer = ammount;
        }
    }

    public void AddGood(int id, int amount)
    {
        // Try to fill maintenance goods first
        amount = UpdateStockpileInList(workplace.maintenanceGoods, id, amount);

        // If there is still amount left, try to fill input goods
        if (production.requiresInput && amount > 0)
        {
            UpdateStockpileInList(production.inputGoods, id, amount);
        }
        //TODO no ammount should be left raise error
    }

    public MarketBuyRequest? BuyMaintenanceGood()
    {
        if (!workplace.HaveAllMaintenanceGoods())
        {
            MarketBuyRequest request = new MarketBuyRequest();
            request.id = id;
            request.marketId = marketId;
            request.cashAmount = production.cashBuffer;
            request.domain = RequestDomain.Building;
            request.GoodRequests = new List<GoodRequest>();
            foreach (GoodRequirement good in workplace.maintenanceGoods)
            {
                int goodId = good.good_id;
                int ammount = (good.maxNeed * size) - good.stockpile;
                if (ammount > 0)
                {
                GoodRequest gr = new GoodRequest(goodId, ammount);
                request.GoodRequests.Add(gr);
                }
            }
            return request;
        }
        return null;
    }

    public void Degrade()
    {
        throw new System.NotImplementedException();
    }


    public int GetWorkAvailableByJobType(PopJob popJob)
    {
        return workplace.GetWorkAvailableByJobType(popJob);
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

    public List<IdNum> PayOwners()
    {
        throw new System.NotImplementedException();
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
        // change building definition (Stellaris like)
        throw new System.NotImplementedException();
    }

    public void Extend() 
    { 
        /*
          Try to buy construction cost 
        once all bought + ic cost =>
            Size += 1
         */
    }
    public void Reduce()
    {
        /*
            Size -= 1
         */
    }

    public int Produce()
    {
        int baseOutput = production.baseOutput;
        int workerRatio = workplace.SmallestProducerWorkerRatio();
        int smallestGoodRatio = 1;

        if (production.requiresInput)
        {
            production.GetsmallestInputGoodRatio(size);
        }

        int ratio = 100;
        if(workerRatio < ratio)
        {
            ratio = workerRatio;
        }
        if(smallestGoodRatio < ratio)
        {
            ratio = smallestGoodRatio;
        }

        int output = (int)(baseOutput * production.efficiency * size) * (ratio / 100);


        //remove consume input
        if (production.requiresInput && ratio > 0)
        {
            production.RemoveInputRatio(ratio);
        }

        return output;
            
       }

    //---- Private ------

    private int UpdateStockpileInList(IEnumerable<GoodRequirement> list, int id, int amount)
    {
        foreach (var gr in list)
        {
            if (gr.good_id == id)
            {
                int total = gr.stockpile + amount;

                if (total <= gr.maxNeed*size)
                {
                    gr.stockpile = total;
                    return 0; // Everything fits, nothing left to carry over
                }
                else
                {
                    gr.stockpile = gr.maxNeed*size;
                    return total - gr.maxNeed; // Return the overflow amount
                }
            }
        }
        return amount; // ID wasn't found in this list, return original amount
    }

    
}


