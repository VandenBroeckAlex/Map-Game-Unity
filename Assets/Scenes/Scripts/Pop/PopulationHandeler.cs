using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static Market_object;
using static Pop_objects;
using System.Linq;


public class PopulationHandeler : MonoBehaviour
{
    private MarketManager marketManager;
    public int test_population_size = 100;

    

    [SerializeField] public List<Pop> populationList = new();

    public float base_growth_rate = 0.004f;
    public float base_consumption = 1f;
    public float base_production = 1.1f;
    private void OnEnable()
    {
        Tick_script.onTick += PopBuy;
        Tick_script.onTick += PopSell;
        DateHandeler.onMonth += PopGrowth;
        DateHandeler.onMonth += ResetPopStockpile;
    }

    private void OnDisable()
    {
        Tick_script.onTick += PopBuy;
        DateHandeler.onMonth -= PopGrowth;
        DateHandeler.onMonth -= ResetPopStockpile;
    }

    private void Awake()
    {
        marketManager = MarketManager.Instance;
    }

    private void Start()
    {
        PopGood[] StockPile = new PopGood[1];
        StockPile[0] = new PopGood();
        StockPile[0].Good_id = 1;
        StockPile[0].MaxNeed = 5;
        StockPile[0].Stockpile = 0;


        populationList.Clear();
        populationList.Add(new Pop(1, 1000, 1, Population_Type.Farmer, Culture.French, Religion.Catholic, 0f, StockPile));

        //get save or initial to create pop
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
            if (populationList[i].ProvinceId == provinceID)
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
                populationList[i].Size += (int)Math.Round(populationList[i].Size * base_growth_rate);
            //Base Growth Rate × Pop Size × CountryModifiers x provinceModdiefier x popGoodFullFilment every month
            Debug.Log(populationList[i].Size);
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
                popId = populationList[i].Id,
                GoodRequest = new(),
                cashAmount = populationList[i].CashAmount,
                marketId = 0
            };

            float cash = populationList[i].CashAmount;
            for (int j = 0; j < populationList[i].GoodList.Length; j++)
            {
                if (populationList[i].GoodList[j].MaxNeed != populationList[i].GoodList[j].Stockpile)
                {
                    Market_object.GoodBuyRequest request = new()
                    {
                        goodId = populationList[i].GoodList[j].Good_id,
                        amountWanted = (populationList[i].GoodList[j].MaxNeed * (populationList[i].Size / 1000)) - populationList[i].GoodList[j].Stockpile
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
        List<MarketResponse> market_awnser = marketManager.Pop_Buy_batch(PopBuyBatchRequest);

        // market awnser with needs fill and money left
        // assign the right values to the right pop
        for (int i = 0; i < market_awnser.Count; i++)
        {
            Pop pop = populationList
            .FirstOrDefault(p => p.Id == market_awnser[i].popId);
            pop.CashAmount = market_awnser[i].cashLeft;
        
            for (int j = 0; j < market_awnser[i].goodsBought.Count; j++) 
            {
                int good_id = market_awnser[i].goodsBought[j].goodId;
                float ammount_bought = market_awnser[i].goodsBought[j].amountBought;

                PopGood popGood = pop.GoodList.FirstOrDefault(g => g.Good_id == good_id);

                popGood.Stockpile += ammount_bought;
                Debug.Log($"Pop have bougt {ammount_bought}/{popGood.MaxNeed}  wood, it have  {pop.CashAmount}$ left");
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
                popId = populationList[i].Id,
                goodSell = new(),
                marketId = 0
            };
            PopRequest.goodSell.goodId = 1;
            PopRequest.goodSell.amountsell = (populationList[i].Size * base_production)/1000;
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
            .FirstOrDefault(p => p.Id == market_awnser[i].popId);

            pop.CashAmount += pop_cash_recived;
        }

    }

    private void ResetPopStockpile()
    {
        for( int i=0; i < populationList.Count; i++)
        {
         
                for(int j = 0; j < populationList[i].GoodList.Length; j++)
                {
                populationList[i].GoodList[j].Stockpile = 0;
                }

        }
        
    }
   
}






