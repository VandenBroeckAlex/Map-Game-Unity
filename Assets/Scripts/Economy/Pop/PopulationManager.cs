using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using static Market_object;
using static Pop_objects;
using static Workplace;


public class PopulationManager
{

    private GameContext context;
    public int test_population_size = 100;
    public static List<PopJob> jobTypes = new List<PopJob>(); 
    

    [SerializeField] private List<Pop> populationList = new();
    // ------ Put those in a json and load them -------
    public float base_growth_rate = 0.004f;
    public float base_consumption = 1f;
    public float base_production = 0.11f;
    // ----------------------------------

    // ----------------------------------



    // ----------------------------------

    public PopulationManager(GameContext context)
    {
        this.context = context;
    }


    public void InitializePopulation()
    {

        List<GoodRequirement> StockPile = new List<GoodRequirement>();

        GoodRequirement good1 = new GoodRequirement();
        good1.Good_id = 1;
        good1.MaxNeed = 100;
        good1.Stockpile = 0;
        StockPile.Add(good1);

        GoodRequirement good2 = new GoodRequirement();
        good2.Good_id = 2;
        good2.MaxNeed = 100;
        good2.Stockpile = 0;
        StockPile.Add(good2);


        populationList.Clear();
        for (int i = 0; i < 1000; i++) 
        {
            Pop newPop = new Pop(1, 1000, 1, new PopJob("Miner", "poor"), Culture.French, Religion.Catholic, 999999999, StockPile);
            newPop.countryID = GetPopCountryByProvinceId(1);
            populationList.Add(newPop);
        }
        Debug.Log("The number of pop in the list is :" + populationList.Count);
    }


    public void PopGrowth()
    {
        for (int i = 0; i < populationList.Count; i++)
        {
            // change pop MaxNeed at the same time (popsize / 1000) * MaxNeed
            if (populationList[i].HaveBasicNeed())
            {
                int pcId = populationList[i].countryID;
                int ppId = populationList[i].provinceId;
                float growRate =  base_growth_rate + ( 1 * context.countriesManager.countryList[pcId].stats.GetPopulationGrowth()) + context.provincesManager.provinces_list[ppId].stats.GetPopulationGrowth();
                Debug.Log($"grow rate = {context.countriesManager.countryList[pcId].stats.GetPopulationGrowth()}");
                int newPopulation = (int)Math.Round(populationList[i].size * growRate);
                populationList[i].size += newPopulation;
                Debug.Log(populationList[i].size);
                Debug.Log($"the pop have grown with {newPopulation} people!");
            }
                
            //Base Growth Rate × Pop Size × CountryModifiers x provinceModdiefier x popGoodFullFilment every month
            Debug.Log(populationList[i].size);
            Debug.Log("the pop have grown !");
        }
    }

    public void PopBuyBatch()
    {
       
        DateTime before = DateTime.Now;
        for (int ii = 0; ii < 5; ii++)
        {
            List<MarketBuyRequest> PopBuyBatchRequest = new();
            
            for (int i = 0; i < populationList.Count; i++)
            {
                Market_object.MarketBuyRequest PopRequest = new()
                {
                    Id = populationList[i].id,
                    GoodRequest = new(),
                    cashAmount = populationList[i].cashAmount,
                    marketId = populationList[i].countryID
                };

                //Debug.Log($"the pop need {populationList[i].GoodList.Count} type of good");
                for (int j = 0; j < populationList[i].GoodList.Count; j++)
                {
                    //Debug.Log($"good id : {populationList[i].GoodList[j].Good_id} , Max need : {populationList[i].GoodList[j].MaxNeed} : stockpile :{populationList[i].GoodList[j].Stockpile}");
                    if (populationList[i].GoodList[j].MaxNeed != populationList[i].GoodList[j].Stockpile)
                    {
                        int goodId = populationList[i].GoodList[j].Good_id;
                        int amountWanted = (populationList[i].GoodList[j].MaxNeed * (populationList[i].size / 1000)) - populationList[i].GoodList[j].Stockpile;
                        Market_object.GoodBuyRequest request = new(goodId, amountWanted);

                        PopRequest.GoodRequest.Add(request);
                    }
                }
                //TODO Debug.Log($"the pop try to buy {PopRequest.GoodRequest.Count} type of good");
                if (PopRequest.GoodRequest != null)
                {
                    PopBuyBatchRequest.Add(PopRequest);
                }

            }
            //call MarketManager with PopBuyBatchRequest
            List<MarketBuyResponse> market_awnser = context.marketManager.BatchMarketBuy(PopBuyBatchRequest);

            // market awnser with needs fill and money left
            // assign the right values to the right pop
            for (int i = 0; i < market_awnser.Count; i++)
            {
                Pop pop = populationList
                .FirstOrDefault(p => p.id == market_awnser[i].id);
                pop.cashAmount = market_awnser[i].cashLeft;

                for (int j = 0; j < market_awnser[i].goodsBought.Count; j++)
                {
                    int good_id = market_awnser[i].goodsBought[j].goodId;
                    int ammount_bought = market_awnser[i].goodsBought[j].amountBought;

                    GoodRequirement popGood = pop.GoodList.FirstOrDefault(g => g.Good_id == good_id);

                    popGood.Stockpile += ammount_bought;
                   //TODO Debug.Log($"Pop have bougt {ammount_bought}/{popGood.MaxNeed - popGood.Stockpile}  wood, it have  {pop.cashAmount}$ left");
                }

            }
        }
        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("BuyBatch Duration in milliseconds: " + duration.Milliseconds);
    }


    public void PopListBuy()
    {

        DateTime before = DateTime.Now;
        for (int i = 0; i < populationList.Count; i++)
        {
               
            Market_object.MarketBuyRequest PopRequest = new()
            {
                Id = populationList[i].id,
                GoodRequest = new(),
                cashAmount = populationList[i].cashAmount,
                marketId = populationList[i].countryID
            };
            //get need
            for (int j = 0; j < populationList[i].GoodList.Count; j++)
            {
                //Debug.Log($"good id : {populationList[i].GoodList[j].Good_id} , Max need : {populationList[i].GoodList[j].MaxNeed} : stockpile :{populationList[i].GoodList[j].Stockpile}");
                if (populationList[i].GoodList[j].MaxNeed != populationList[i].GoodList[j].Stockpile)
                {
                    int goodId = populationList[i].GoodList[j].Good_id;
                    int amountWanted = (populationList[i].GoodList[j].MaxNeed * (populationList[i].size / 1000)) - populationList[i].GoodList[j].Stockpile;
                    Market_object.GoodBuyRequest request = new(goodId, amountWanted);

                    PopRequest.GoodRequest.Add(request);
                }
            }
            //call market
            MarketBuyResponse market_awnser = context.marketManager.MarketBuy(PopRequest);
            //distribute 
            Pop pop = populationList[i];
            pop.cashAmount = market_awnser.cashLeft;

            for (int j = 0; j < market_awnser.goodsBought.Count; j++)
            {
                int good_id = market_awnser.goodsBought[j].goodId;
                int ammount_bought = market_awnser.goodsBought[j].amountBought;

                GoodRequirement popGood = pop.GoodList.FirstOrDefault(g => g.Good_id == good_id);

                popGood.Stockpile += ammount_bought;
                //TODO Debug.Log($"Pop have bougt {ammount_bought}/{popGood.MaxNeed - popGood.Stockpile}  wood, it have  {pop.cashAmount}$ left");
            }
        }
        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
    }

    //This should be removed
    //public void PopSell() 
    //{ 
    //    List<MarketSellRequest> PopSellBatchRequest = new();
    //    for (int i = 0; i < populationList.Count; i++)
    //    {
    //            int Id = populationList[i].id,

    //            int marketId = populationList[i].countryID
    //        Market_object.MarketSellRequest PopRequest = new()

    //        PopRequest.goodSell.goodId = 1;
    //        PopRequest.goodSell.amountsell = (int)(((populationList[i].size * base_production)/1000)*100);
    //        Debug.Log($"Pop have sell an amount of {PopRequest.goodSell.amountsell} ");
    //        PopSellBatchRequest.Add(PopRequest);
    //    }
    //    //call marketSell
    //    List<MarketSellResponse> market_awnser = context.marketManager.Pop_Sell(PopSellBatchRequest);
    //    //re-assign cash to pop

    //    for (int i = 0; i < market_awnser.Count; i++)       
    //    {
    //       int pop_id = market_awnser[i].Id;
    //       int pop_cash_recived = market_awnser[i].cashRecived;

    //        Pop pop = populationList
    //        .FirstOrDefault(p => p.id == market_awnser[i].Id);

    //        pop.cashAmount += pop_cash_recived;
    //    }

    //}
    public void PayPop(List<IdNum> info)
    {
        foreach (IdNum idnum in info) 
        { 
            Pop pop = populationList.Where( p=> p.id == idnum.id).FirstOrDefault();
            if (pop != null) 
            {
                pop.cashAmount += idnum.num;
            }
        }
    }


    public void ResetPopStockpile()
    {
        for( int i=0; i < populationList.Count; i++)
        {
                for(int j = 0; j < populationList[i].GoodList.Count; j++)
                {
                populationList[i].GoodList[j].Stockpile = 0;
                }
        }
    }

    public void PopHired(int popId, int ammount, int workplaceId)
    {
        Pop pop = GetPopById(popId);
        pop.HireInWorkplace(workplaceId,ammount);
    }

    public void PopFired(int popId, int ammount, int workplaceId)
    {
        Pop pop = GetPopById(popId);
        pop.FiredFromWorkplace(workplaceId, ammount);
    }

    private Pop GetPopById(int id)
    {
        return populationList.Where(p => p.id == id).First();
    }
    private int GetPopCountryByProvinceId(int provinceID)
    {
        int countryID = context.provincesManager.GetProvinceOwnerByProvinceId(provinceID);
        return countryID;
    }

    public void PopSearchWork()
    {
        // if unemployed and workplace with space => get employed
        for (int i = 0; i < populationList.Count; i++) 
        {
            Pop pop = populationList[i];
            int unemployedNum = pop.GetUnemployedNumber();

            if (unemployedNum > 0) 
            {
                //get workplace in pop province
                List<IWorkplace> workplaceList = context.workplaceManager.GetWorkplaceByProvinceId(pop.provinceId);

                foreach(IWorkplace workplace in workplaceList)
                {
                    int availableWork = workplace.GetWorkAvailableByJobType(pop.job);

                    if (availableWork > 0) 
                    {
                        int workplaceId = workplace.GetWorkplaceId();
                        if (availableWork >= unemployedNum)
                        {
                            PopHired(pop.id, unemployedNum, workplace.GetWorkplaceId());
                            context.workplaceManager.WorkplaceHire(workplaceId, pop.id, unemployedNum, pop.job);
                            break;
                        }
                        else
                        {
                            unemployedNum -= availableWork;
                            PopHired(pop.id, availableWork, workplace.GetWorkplaceId());
                            context.workplaceManager.WorkplaceHire(workplaceId, pop.id, availableWork, pop.job);
                        }
                    }
                }
            }

        }
         
        // future: check for different workplace wages
    }


    //--------- For UI ---------
    public List<Pop> SelectPopulationByCountry(int countryId)
    {
        return populationList.Where(p => p.countryID == countryId).ToList();
    }
    public List<Pop> SelectPopulationByProvince(int provinceID)
    {
        List<Pop> selectedPops = new();
        for (int i = 0; i < populationList.Count; i++)
        {
            if (populationList[i].provinceId == provinceID)
            {
                selectedPops.Add(populationList[i]);
            }
        }
        return selectedPops;
    }


}






