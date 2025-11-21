using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static Goods;
using static Market_object;
using static UnityEngine.EventSystems.EventTrigger;
using static helpers_math;
using static Goods_loader;
using static CountriesManager;




public class MarketManager : MonoBehaviour
{

    private CountriesManager _countriesManager;
    public static MarketManager instance { get; private set; }  

  

    [SerializeField] public Market WorldMarket = new Market();
    List<Goods.Good> good_definition_list = new List<Goods.Good>();

    public Dictionary<int,Market> marketList = new Dictionary<int,Market>();

    public float priceSensitivity = 0.1f;

    private void OnEnable()
    { 
        DateHandeler.onMonth += PriceFluctuation;
    }

    private void OnDisable()
    {
        DateHandeler.onMonth -= PriceFluctuation;
    }

   

    public void Initialize()
    {
        _countriesManager = CountriesManager.instance;
        CreateSingleton();
        InitializeGoods();
        WorldMarket = CreateMarket();
        InitializeCountryMarket();

        Debug.Log($"Their is {marketList.Count} markets in the list");
    }

    private void CreateSingleton()
    {
        // Singleton pattern: only one instance allowed
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("An instance of market manager already exist");
            Destroy(gameObject);
        }
    }
    private void InitializeGoods()
    {
        Debug.Log("Is initializing goods");
        good_definition_list = Load_goods();
        Debug.Log($"{good_definition_list.Count} goods have been loaded");

        for (int i = 0; i < good_definition_list.Count; i++)
        {
            Goods.Good good = new Goods.Good();
            good.id = good_definition_list[i].id;
            good.basePrice = good_definition_list[i].basePrice;
            good.name = good_definition_list[i].name;
            good.weight = good_definition_list[i].weight;
            good.type = good_definition_list[i].type;

            WorldMarket.goods_list.Add(new MarketGood
            {
                good = good,
                supply = 0f,
                demand = 0f,
                price = 1f
            });

        }
        //Debug.Log(globalMarket.goods_list[30].good.name);
    }


    public List<MarketResponse> Pop_Buy_batch(List<MarketBuyRequest> PopRequestBatch)
    {
        List<MarketResponse> marketResponse = new List<MarketResponse>();
       
        
        for (int i = 0; i < PopRequestBatch.Count; i++) 
        {
            //construction of response object
            MarketResponse response = new MarketResponse();
           
            response.popId = PopRequestBatch[i].popId;
            response.goodsBought = new List<GoodResponse>();

            

            int marketId = PopRequestBatch[i].marketId;
            float cashAmount = PopRequestBatch[i].cashAmount;
            response.cashLeft = cashAmount;



            for (int j = 0; j < PopRequestBatch[i].GoodRequest.Count; j++)
            {
                GoodResponse goodResponse = new GoodResponse();

                int goodId = PopRequestBatch[i].GoodRequest[j].goodId;

                goodResponse.goodId = goodId;

               

                Market_object.MarketGood Marketgood = WorldMarket.goods_list
                .Where(good => good.good.id == goodId).FirstOrDefault();



                float amountWanted = PopRequestBatch[i].GoodRequest[j].amountWanted;
                float totalCost = Marketgood.price * amountWanted;
                if(cashAmount == 0)
                {
                    break;
                }



                if (totalCost <= cashAmount)
                {
                    // Can afford the full amount
                    goodResponse.amountBought = RoundToTwoDecimals(amountWanted);
                    response.cashLeft -= RoundToTwoDecimals(totalCost);
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("full amount");
                    Debug.Log("cash left =" + response.cashLeft);

                    Marketgood.demand += RoundToTwoDecimals(amountWanted);
                    Marketgood.stockpile -= RoundToTwoDecimals(amountWanted);   
                    
                }
                else
                {
                    // Can only afford partial amount
                    float amountAffordable = Mathf.Floor(cashAmount / Marketgood.price * 10f) / 10f;
                    goodResponse.amountBought = RoundToTwoDecimals(amountAffordable);
                    response.cashLeft = 0;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("partial amount");

                    Marketgood.demand += RoundToTwoDecimals(amountWanted);
                    Marketgood.stockpile -= RoundToTwoDecimals(amountAffordable);

                    break; // Exit loop early, no cash left
                }
                
          
            }
            marketResponse.Add(response);
        }
        // Market curentMarket = marketList.FirstOrDefault(market => market.id == marketId);

        return marketResponse;
    }


    public List<MarketSellResponse> Pop_Sell_batch(List<MarketSellRequest> marketSellRequestList)
    {
        List<MarketSellResponse>  batch_response = new List<MarketSellResponse>();

        for (int i = 0; i < marketSellRequestList.Count; i++)
        {
            MarketSellResponse response = new MarketSellResponse();
            response.popId = marketSellRequestList[i].popId;

            int goodId = marketSellRequestList[i].goodSell.goodId;

                Market_object.MarketGood Marketgood = WorldMarket.goods_list
               .Where(good => good.good.id == goodId).FirstOrDefault();

            response.cashRecived = RoundToTwoDecimals(Marketgood.price * marketSellRequestList[i].goodSell.amountsell);
            batch_response.Add(response);

            Marketgood.supply += RoundToTwoDecimals(marketSellRequestList[i].goodSell.amountsell);
            Marketgood.stockpile += RoundToTwoDecimals(marketSellRequestList[i].goodSell.amountsell);


        }
        return batch_response;
    }



    private void PriceFluctuation()
    {

        for (int i = 0; i < WorldMarket.goods_list.Count; i++) 
        {

            WorldMarket.goods_list[i].RecordGoodHistory();

            float supply = WorldMarket.goods_list[i].supply;
            float demand = WorldMarket.goods_list[i].demand;
            
            float old_price = WorldMarket.goods_list[i].price;
             if ((demand - supply) > 0)
            {       
                WorldMarket.goods_list[i].price += WorldMarket.goods_list[i].price * priceSensitivity;
            }
            else if(demand == supply)
            {
                
            }
            else
            {   
                WorldMarket.goods_list[i].price -= WorldMarket.goods_list[i].price * priceSensitivity;
            }

            //reset supply and demand beggening of the month
            WorldMarket.goods_list[i].supply = 0;
            WorldMarket.goods_list[i].demand = 0;
            Debug.Log("supply:" + supply);
            Debug.Log("demand:" + demand);
            Debug.Log($"{WorldMarket.goods_list[i].good.name}, price is now : {WorldMarket.goods_list[i].price}, it have change of {WorldMarket.goods_list[i].price - old_price}$");
        }
    }


    private Market CreateMarket()
    {
        Market Market = new Market();
        Market.goods_list = new List<MarketGood>();

        foreach (Goods.Good _good in good_definition_list) 
        { 
            MarketGood _mg = new MarketGood();
            _mg.id = _good.id;
            _mg.good = _good;
            _mg.price = 1;
            _mg.supply = 0;
            _mg.demand = 0;
            _mg.stockpile = 100;
            //Debug.Log($"Adding {_mg.good.name} to the market");
            Market.goods_list.Add(_mg);
        }

        return Market;
    }

    private void CreateCountryMarket(int countryId)
    {
        Market market = new Market();
        market = CreateMarket();
        market.id = countryId;
        marketList.Add(countryId, market);

    }

    private void InitializeCountryMarket()
    {
        //foreach country in country manager
        foreach(KeyValuePair<int,Country> kv in _countriesManager.countryList)
        {
            CreateCountryMarket(kv.Key);
        }

    }

}



/*
1 Production Phase:

    Buildings produce goods.

    Output goes into the global (or local) market.

    Needs Assessment Phase:

    Pops calculate their demand based on size.

    Wealth limits what they can actually buy.

2 Trade Phase:

    Goods are matched with demand.

    Prices adjust.

    Wealth is transferred from consumers to producers.

4 Population Update Phase:

    Unmet needs lower happiness.

    Overconsumption can raise class mobility or birthrate.

    Wealth growth/shrinkage is tracked.
 */