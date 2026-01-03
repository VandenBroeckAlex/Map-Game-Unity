using MyGame.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Goods_loader;
using static Market_object;
using static GoodDatabase;




public class MarketManager : MonoBehaviour
{
    private GameContext context;


    //[SerializeField] public Market worldMarket = new Market();
    List<Good> good_definition_list;
    List<MarketGood> default_market_goods = new List<MarketGood>();

    public Dictionary<int,Market> marketList = new Dictionary<int,Market>();

    public event Action OnMarketUpdated;

    public float priceSensitivity = 0.1f;

 



    public MarketManager(GameContext context)
    {
        this.context = context;
    }
   

    public void Initialize()
    {
        good_definition_list = GoodDatabase.good_definition_list;
        default_market_goods = CreateMarketGoodTemplate();
        //worldMarket = CreateMarket();
        InitializeCountryMarket();
        Debug.Log($"Theirs is {default_market_goods.Count} marketGood");
        Debug.Log($"Their is {marketList.Count} markets in the list");
    }
    

    private List<MarketGood> CreateMarketGoodTemplate()
    {
        var template = new List<MarketGood>();

        foreach (var g in good_definition_list)
        {
            template.Add(new MarketGood
            {
                id = g.id,
                good = g,
                price = g.basePrice,
                supply = 0,
                demand = 0,
                stockpile = 0
            });
        }

        return template;
    }

    private List<MarketGood> CloneGoodsTemplate(List<MarketGood> template)
    {
        var list = new List<MarketGood>();

        foreach (var item in template)
        {
            list.Add(new MarketGood
            {
                id = item.id,
                good = item.good,           // shared definition
                price = item.price,         // copied
                supply = 0,
                demand = 0,
                stockpile = 0
            });
        }

        return list;
    }


    public List<MarketBuyResponse> BatchMarketBuy(List<MarketBuyRequest> marketBuyRequestList)
    {
        List<MarketBuyResponse> marketResponse = new List<MarketBuyResponse>();
       
        
        for (int i = 0; i < marketBuyRequestList.Count; i++) 
        {
            //construction of response object
            MarketBuyResponse response = new MarketBuyResponse();
           
            response.id = marketBuyRequestList[i].Id;
            response.goodsBought = new List<GoodResponse>();

            int market_id = marketBuyRequestList[i].marketId;
            Market _market = marketList[market_id];
            int cashAmount = marketBuyRequestList[i].cashAmount;
            response.cashLeft = cashAmount;


            for (int j = 0; j < marketBuyRequestList[i].GoodRequest.Count; j++)
            {
                GoodResponse goodResponse = new GoodResponse();

                int goodId = marketBuyRequestList[i].GoodRequest[j].goodId;

                goodResponse.goodId = goodId;

                Market_object.MarketGood Marketgood = _market.goods_list
                .Where(good => good.good.id == goodId).FirstOrDefault();
           

                int amountWanted = marketBuyRequestList[i].GoodRequest[j].amountWanted;
                //TODO Debug.Log($"price : {Marketgood.price}, ammount wanted : {amountWanted} ");
                int totalCost = Marketgood.price * amountWanted;
                if(cashAmount == 0)
                {
                    break;
                    //popo
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

                    Marketgood.demand += amountWanted + 1;
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


    public List<MarketSellResponse> BatchMarketSell(List<MarketSellRequest> marketSellRequestList)
    {
        List<MarketSellResponse> batch_response = new List<MarketSellResponse>();

        for (int i = 0; i < marketSellRequestList.Count; i++)
        {
            //create the response object
            MarketSellResponse response = new MarketSellResponse();
            response.Id = marketSellRequestList[i].Id;
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
            float country_income_tax = context.countriesManager.countryList[_market.countryId].Income_tax;

            int country_income = (int)(BrutCash * country_income_tax); //country_income_tax * province controle * admin capacity

            context.countriesManager.countryList[_market.countryId].ReceiveCash(country_income);

            // NetCash
            response.cashRecived = BrutCash - country_income;
            batch_response.Add(response);

            _market.AddGoodStockpile(Marketgood.good.id, marketSellRequestList[i].goodSell.amountsell);
        }

        OnMarketUpdated?.Invoke();//call refresh ui
        return batch_response;
    }


    private void ChangeMarketIncomeTax(int countryId, float tax)
    {
        Market market = marketList[countryId];

        market.SetOwnerIncomeTax(tax);
    }


    public void PriceFluctuation()
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
                Debug.Log($"{market.goods_list[i].good.name}, price is now : {market.goods_list[i].price}, it have change of {market.goods_list[i].price - old_price}$");
            }
        }

    }


    public MarketSellResponse MarketSell(MarketSellRequest sellRequest)
    {
        //create the response object
        MarketSellResponse response = new MarketSellResponse();
        response.Id = sellRequest.Id;
        int goodId = sellRequest.goodSell.goodId;


        //search for market
        int market_id = sellRequest.marketId;
        Market _market = marketList[market_id];

        //search for good
        Market_object.MarketGood Marketgood = _market.goods_list
        .Where(good => good.good.id == goodId).FirstOrDefault();


        //country tax
        int BrutCash = Marketgood.price * sellRequest.goodSell.amountsell;

        //TODO change this !
        //country tax
        float country_income_tax = context.countriesManager.countryList[_market.countryId].Income_tax;

        int country_income = (int)(BrutCash * country_income_tax); //country_income_tax * province controle * admin capacity

        context.countriesManager.countryList[_market.countryId].ReceiveCash(country_income);

        // NetCash
        response.cashRecived = BrutCash - country_income;

        _market.AddGoodStockpile(Marketgood.good.id, sellRequest.goodSell.amountsell);

        return response;    
    }


    public MarketBuyResponse MarketBuy(MarketBuyRequest request)
    {
        MarketBuyResponse response = new MarketBuyResponse();

        response.id = request.Id;
        response.goodsBought = new List<GoodResponse>();

        int market_id = request.marketId;
        Market _market = marketList[market_id];
        int cashAmount = request.cashAmount;
        response.cashLeft = cashAmount;


        for (int j = 0; j < request.GoodRequest.Count; j++)
        {
            GoodResponse goodResponse = new GoodResponse();

            int goodId = request.GoodRequest[j].goodId;

            goodResponse.goodId = goodId;

            Market_object.MarketGood Marketgood = _market.goods_list
            .Where(good => good.good.id == goodId).FirstOrDefault();


            int amountWanted = request.GoodRequest[j].amountWanted;
            Debug.Log($"price : {Marketgood.price}, ammount wanted : {amountWanted} ");
            int totalCost = Marketgood.price * amountWanted;
            if (cashAmount == 0)
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

                _market.SubstractGoodStockpile(Marketgood.good.id, amountWanted);
            }
            else
            {
                // Can only afford partial amount
                int amountAffordable = cashAmount / Marketgood.price;
                goodResponse.amountBought = amountAffordable;
                response.cashLeft = 0;
                response.goodsBought.Add(goodResponse);
                Debug.Log("partial amount");

                Marketgood.demand += amountWanted;
                Marketgood.stockpile -= amountAffordable;

                break; // Exit loop early, no cash left
            }
        }
        OnMarketUpdated?.Invoke();//call refresh ui
        return response;
    }

    

    public Market GetMarketByCountryId(int countryId)
    {
        return marketList[countryId];
    }


    private Market CreateMarket()
    {
        Market market = new Market();
        market.goods_list = CloneGoodsTemplate(default_market_goods);
        return market;
    }


    private void InitializeCountryMarket()
    {
        //foreach country in country manager
        foreach(KeyValuePair<int,Country> kv in context.countriesManager.countryList)
        {
            CreateCountryMarket(kv.Key);
        }

    }
    private void CreateCountryMarket(int countryId)
    {
        Market market = new Market();
        market = CreateMarket();
        market.id = countryId;
        market.countryId = countryId;
        marketList.Add(countryId, market);
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