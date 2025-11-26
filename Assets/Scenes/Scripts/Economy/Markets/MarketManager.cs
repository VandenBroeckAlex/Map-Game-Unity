using MyGame.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static CountriesManager;
using static Goods;
using static Goods_loader;
using static helpers_math;
using static Market_object;
using static UnityEngine.EventSystems.EventTrigger;




public class MarketManager : MonoBehaviour
{

    private CountriesManager _countriesManager;
    public static MarketManager instance { get; private set; }  

    

    //[SerializeField] public Market worldMarket = new Market();
    List<Goods.Good> good_definition_list = new List<Goods.Good>();
    List<MarketGood> default_market_goods = new List<MarketGood>();

    public Dictionary<int,Market> marketList = new Dictionary<int,Market>();

    public event Action OnMarketUpdated;

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
        Debug.Log("Is initializing goods");
        good_definition_list = Load_goods();
        InitializeDefaultMarketGoods();
        //worldMarket = CreateMarket();
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

    private void InitializeDefaultMarketGoods()
    {
        
        Debug.Log($"{good_definition_list.Count} goods have been loaded");

        for (int i = 0; i < good_definition_list.Count; i++)
        {
            Goods.Good good = new Goods.Good();
            good.id = good_definition_list[i].id;
            good.basePrice = good_definition_list[i].basePrice;
            good.name = good_definition_list[i].name;
            good.weight = good_definition_list[i].weight;
            good.type = good_definition_list[i].type;

            default_market_goods.Add(new MarketGood
            {
                good = good,
                supply = 0,
                demand = 0,
                price = 100
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



            int market_id = PopRequestBatch[i].marketId;
            Market _market = marketList[market_id];
            int cashAmount = PopRequestBatch[i].cashAmount;
            response.cashLeft = cashAmount;




            for (int j = 0; j < PopRequestBatch[i].GoodRequest.Count; j++)
            {
                GoodResponse goodResponse = new GoodResponse();

                int goodId = PopRequestBatch[i].GoodRequest[j].goodId;

                goodResponse.goodId = goodId;

               

                Market_object.MarketGood Marketgood = _market.goods_list
                .Where(good => good.good.id == goodId).FirstOrDefault();
           

                int amountWanted = PopRequestBatch[i].GoodRequest[j].amountWanted;
                int totalCost = Marketgood.price * amountWanted;
                if(cashAmount == 0)
                {
                    break;
                }



                if (totalCost <= cashAmount)
                {
                    // Can afford the full amount
                    goodResponse.amountBought = amountWanted;
                    response.cashLeft -= totalCost;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("full amount");
                    Debug.Log("cash left =" + response.cashLeft);

                    Marketgood.demand += amountWanted;
                    Marketgood.stockpile -= amountWanted;   
                    
                }
                else
                {
                    // Can only afford partial amount
                    int amountAffordable = cashAmount / Marketgood.price ;
                    goodResponse.amountBought = amountAffordable;
                    response.cashLeft = 0;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("partial amount");

                    Marketgood.demand += amountWanted;
                    Marketgood.stockpile -= amountAffordable;

                    break; // Exit loop early, no cash left
                }
                
          
            }
            marketResponse.Add(response);
        }
        // Market curentMarket = marketList.FirstOrDefault(market => market.id == marketId);

        OnMarketUpdated?.Invoke();//call refresh ui
        return marketResponse;
    }


    public List<MarketSellResponse> Pop_Sell(List<MarketSellRequest> marketSellRequestList)
    {
        List<MarketSellResponse> batch_response = new List<MarketSellResponse>();

        for (int i = 0; i < marketSellRequestList.Count; i++)
        {
            //create the response object
            MarketSellResponse response = new MarketSellResponse();
            response.popId = marketSellRequestList[i].popId;
            int goodId = marketSellRequestList[i].goodSell.goodId;



            //search for market
            int market_id = marketSellRequestList[i].marketId;
            Market _market = marketList[market_id];

            //search for good
            Market_object.MarketGood Marketgood = _market.goods_list
            .Where(good => good.good.id == goodId).FirstOrDefault();


            //country tax
            int BrutCash = Marketgood.price * marketSellRequestList[i].goodSell.amountsell;

            //TODO change this !
            //country tax
            float country_income_tax = _countriesManager.countryList[_market.countryId].Income_tax;

            int country_income = (int)(BrutCash * country_income_tax); //country_income_tax * province controle * admin capacity

            _countriesManager.countryList[_market.countryId].ReceiveCash(country_income);

            // NetCash
            response.cashRecived = BrutCash - country_income;
            batch_response.Add(response);

            Marketgood.supply += marketSellRequestList[i].goodSell.amountsell;
            Marketgood.stockpile += marketSellRequestList[i].goodSell.amountsell;
        }

        foreach (KeyValuePair<int, Country> kv in _countriesManager.countryList)
        {
            Country country = kv.Value;
            Debug.Log($"{country.name} treasury = {country.treasury}");
        }

        OnMarketUpdated?.Invoke();//call refresh ui
        return batch_response;
    }


    public void ChangeMarketIncomeTax(int countryId, float tax)
    {
        Market market = marketList[countryId];

        market.SetOwnerIncomeTax(tax);
    }


    private void PriceFluctuation()
    {
        foreach (KeyValuePair<int, Market> kv in marketList)
        {
            Market market = kv.Value;
            for (int i = 0; i < market.goods_list.Count; i++)
            {

                market.goods_list[i].RecordGoodHistory();

                float supply = market.goods_list[i].supply;
                float demand = market.goods_list[i].demand;

                float old_price = market.goods_list[i].price;

                if ((demand - supply) > 0)
                {
                    market.goods_list[i].price += (int)(market.goods_list[i].price * priceSensitivity);
                }
                else if (demand == supply)
                {

                }
                else
                {
                    market.goods_list[i].price -=   (int)(market.goods_list[i].price * priceSensitivity);
                }

                //reset supply and demand beggening of the month
                market.goods_list[i].supply = 0;
                market.goods_list[i].demand = 0;
                //Debug.Log("supply:" + supply);
                //Debug.Log("demand:" + demand);
                //Debug.Log($"{market.goods_list[i].good.name}, price is now : {market.goods_list[i].price}, it have change of {market.goods_list[i].price - old_price}$");
            }
        }

    }


    public Market GetMarketByCountryId(int countryId)
    {
        return marketList[countryId];
    }


    private Market CreateMarket()
    {
        Market Market = new Market();
        Market.goods_list = new List<MarketGood>();
        Market.goods_list = default_market_goods;
        return Market;
    }

    private void CreateCountryMarket(int countryId)
    {
        Market market = new Market();
        market = CreateMarket();
        market.id = countryId;
        market.goods_list = default_market_goods;


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