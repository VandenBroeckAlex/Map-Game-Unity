using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using static Market_object;
using static Pop_objects;


public class PopulationManager : MonoBehaviour
{

    private GameContext context;
    public int test_population_size = 100;
    public static List<JobType> jobTypes = new List<JobType>();
    

    [SerializeField] public List<Pop> populationList = new();
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
        good1.MaxNeed = 500;
        good1.Stockpile = 0;
        StockPile.Add(good1);

        GoodRequirement good2 = new GoodRequirement();
        good2.Good_id = 2;
        good2.MaxNeed = 500;
        good2.Stockpile = 0;
        StockPile.Add(good2);


        populationList.Clear();
        Pop newPop = new Pop(1, 1000, 1, new PopJob("Miner", "poor"), Culture.French, Religion.Catholic, 10, StockPile);
        newPop.countryID = GetPopCountryByProvinceId(1);
        populationList.Add(newPop);

    
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
                float growRate =  base_growth_rate + context.countriesManager.countryList[pcId].stats.GetPopulationGrowth() + context.provincesManager.provinces_list[ppId].stats.GetPopulationGrowth();
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

    public void PopBuy()
    {
        List<MarketBuyRequest> PopBuyBatchRequest = new();
        for (int i = 0; i < populationList.Count; i++)
        {
            Market_object.MarketBuyRequest PopRequest = new()
            {
                popId = populationList[i].id,
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
                    
                    Market_object.GoodBuyRequest request = new()
                    {
                        goodId = populationList[i].GoodList[j].Good_id,
                        amountWanted = (populationList[i].GoodList[j].MaxNeed * (populationList[i].size / 1000)) - populationList[i].GoodList[j].Stockpile
                    };
                    PopRequest.GoodRequest.Add(request);
                }  
            }
            Debug.Log($"the pop try to buy {PopRequest.GoodRequest.Count} type of good");
            if(PopRequest.GoodRequest != null)
            {
                PopBuyBatchRequest.Add(PopRequest);
            }
            
        }
        //call MarketManager with PopBuyBatchRequest
        List<MarketResponse> market_awnser = context.marketManager.Pop_Buy_batch(PopBuyBatchRequest);

        // market awnser with needs fill and money left
        // assign the right values to the right pop
        for (int i = 0; i < market_awnser.Count; i++)
        {
            Pop pop = populationList
            .FirstOrDefault(p => p.id == market_awnser[i].popId);
            pop.cashAmount = market_awnser[i].cashLeft;
        
            for (int j = 0; j < market_awnser[i].goodsBought.Count; j++) 
            {
                int good_id = market_awnser[i].goodsBought[j].goodId;
                int ammount_bought = market_awnser[i].goodsBought[j].amountBought;

                GoodRequirement popGood = pop.GoodList.FirstOrDefault(g => g.Good_id == good_id);

                popGood.Stockpile += ammount_bought;
                Debug.Log($"Pop have bougt {ammount_bought}/{popGood.MaxNeed}  wood, it have  {pop.cashAmount}$ left");
            }
            
        }
        
    }

    //This should be removed
    public void PopSell() 
    { 
        List<MarketSellRequest> PopSellBatchRequest = new();
        for (int i = 0; i < populationList.Count; i++)
        {
            Market_object.MarketSellRequest PopRequest = new()
            {
                popId = populationList[i].id,
                goodSell = new(),
                marketId = populationList[i].countryID
            };
            PopRequest.goodSell.goodId = 1;
            PopRequest.goodSell.amountsell = (int)((populationList[i].size * base_production)/1000)*100;
            Debug.Log($"Pop have sell an amount of {PopRequest.goodSell.amountsell} ");
            PopSellBatchRequest.Add(PopRequest);
        }
        //call marketSell
        List<MarketSellResponse> market_awnser = context.marketManager.Pop_Sell(PopSellBatchRequest);
        //re-assign cash to pop

        for (int i = 0; i < market_awnser.Count; i++)       
        {
           int pop_id = market_awnser[i].popId;
           int pop_cash_recived = market_awnser[i].cashRecived;

            Pop pop = populationList
            .FirstOrDefault(p => p.id == market_awnser[i].popId);

            pop.cashAmount += pop_cash_recived;
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






