using System;
using System.Collections.Generic;
using UnityEngine;
using static MarketManager;
using static Market_object;
using static Pop_objects;
using System.Linq;


public class PopulationManager : MonoBehaviour
{
    private MarketManager marketManager;
    public int test_population_size = 100;

    

    [SerializeField] public List<Pop> populationList = new();
    // ------ Put those in a json and load them -------
    public float base_growth_rate = 0.004f;
    public float base_consumption = 1f;
    public float base_production = 1.1f;
    // ----------------------------------
    private void OnEnable()
    {
        TickScript.onTick += PopBuy;
        TickScript.onTick += PopSell;
        DateHandeler.onMonth += PopGrowth;
        DateHandeler.onMonth += ResetPopStockpile;
    }

    private void OnDisable()
    {
        TickScript.onTick += PopBuy;
        DateHandeler.onMonth -= PopGrowth;
        DateHandeler.onMonth -= ResetPopStockpile;
    }

 

   

    public void InitializePopulation()
    {
        marketManager = MarketManager.instance;

        List<PopGood> StockPile = new List<PopGood>();

        PopGood good1 = new PopGood();
        good1.Good_id = 1;
        good1.MaxNeed = 5;
        good1.Stockpile = 0;
        StockPile.Add(good1);

        populationList.Clear();
        populationList.Add(new Pop(1, 1000, 1, Population_Type.Farmer, Culture.French, Religion.Catholic, 0f, StockPile));
    }

    /*
     public Pop[] SelectPopulationByCountry(int countryId)
     {

     }
    */

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


    private void PopGrowth()
    {
        for (int i = 0; i < populationList.Count; i++)
        {
            // change pop MaxNeed at the same time (popsize / 1000) * MaxNeed
            if (populationList[i].HaveBasicNeed())
                populationList[i].size += (int)Math.Round(populationList[i].size * base_growth_rate);
            //Base Growth Rate × Pop Size × CountryModifiers x provinceModdiefier x popGoodFullFilment every month
            Debug.Log(populationList[i].size);
            Debug.Log("the pop have grow !");
        }
    }


    private void PopBuy()
    {
        List<MarketBuyRequest> PopBuyBatchRequest = new();
        for (int i = 0; i < populationList.Count; i++)
        {
            Market_object.MarketBuyRequest PopRequest = new()
            {
                popId = populationList[i].id,
                GoodRequest = new(),
                cashAmount = populationList[i].cashAmount,
                marketId = 0
            };

            float cash = populationList[i].cashAmount;
            for (int j = 0; j < populationList[i].GoodList.Count; j++)
            {
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
            if(PopRequest.GoodRequest != null)
            {
                PopBuyBatchRequest.Add(PopRequest);
            }
            
        }
        //call MarketManager with PopBuyBatchRequest
        Debug.Log(marketManager);
        List<MarketResponse> market_awnser = marketManager.Pop_Buy_batch(PopBuyBatchRequest);

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
                float ammount_bought = market_awnser[i].goodsBought[j].amountBought;

                PopGood popGood = pop.GoodList.FirstOrDefault(g => g.Good_id == good_id);

                popGood.Stockpile += ammount_bought;
                Debug.Log($"Pop have bougt {ammount_bought}/{popGood.MaxNeed}  wood, it have  {pop.cashAmount}$ left");
            }
            
        }
        
    }
    private void PopSell()
    {
        List<MarketSellRequest> PopSellBatchRequest = new();

        for (int i = 0; i < populationList.Count; i++)
        {
            Market_object.MarketSellRequest PopRequest = new()
            {
                popId = populationList[i].id,
                goodSell = new(),
                marketId = 0
            };
            PopRequest.goodSell.goodId = 1;
            PopRequest.goodSell.amountsell = (populationList[i].size * base_production)/1000;
            PopSellBatchRequest.Add(PopRequest);
        }
        //call marketSell
        List<MarketSellResponse> market_awnser = marketManager.Pop_Sell_batch(PopSellBatchRequest);
        //re-assign cash to pop

        for (int i = 0; i < market_awnser.Count; i++)       
        {
           int pop_id = market_awnser[i].popId;
           float pop_cash_recived = market_awnser[i].cashRecived;

            Pop pop = populationList
            .FirstOrDefault(p => p.id == market_awnser[i].popId);

            pop.cashAmount += pop_cash_recived;
        }

    }

    private void ResetPopStockpile()
    {
        for( int i=0; i < populationList.Count; i++)
        {
         
                for(int j = 0; j < populationList[i].GoodList.Count; j++)
                {
                populationList[i].GoodList[j].Stockpile = 0;
                }

        }
        
    }
   
}






