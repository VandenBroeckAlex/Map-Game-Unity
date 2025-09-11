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





public class MarketManager : MonoBehaviour
{
   
    [SerializeField] public Market global_market = new Market();
    List<Goods.Good> good_definition_list = new List<Goods.Good>();


    public float priceSensitivity = 0.1f;

    private void OnEnable()
    { 
        DateHandeler.onMonth += PriceFluctuation;
    }

    private void OnDisable()
    {
        DateHandeler.onMonth -= PriceFluctuation;
    }



    private void Start()
    {
        // initialisation for game creation (not loading a save)  Market should have different price at game beginning 
        //call goodloader
        good_definition_list = Load_goods();


        for (int i = 0; i <  good_definition_list.Count; i++)
        {
            Goods.Good good = new Goods.Good();
            good.id = good_definition_list[i].id;
            good.basePrice = good_definition_list[i].basePrice;
            good.name = good_definition_list[i].name;
            good.weight = good_definition_list[i].weight;
            good.type = good_definition_list[i].type;

            global_market.goods_list.Add(new Market_good
            {
                good = good,
                supply = 0f,
                demand = 0f,
                price = 1f
            });
   
        }
        Debug.Log(global_market.goods_list[30].good.name);
    
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

               

                Market_object.Market_good Marketgood = global_market.goods_list
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

                Market_object.Market_good Marketgood = global_market.goods_list
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

        for (int i = 0; i < global_market.goods_list.Count; i++) 
        {

            global_market.goods_list[i].RecordGoodHistory();

            float supply = global_market.goods_list[i].supply;
            float demand = global_market.goods_list[i].demand;
            
            float old_price = global_market.goods_list[i].price;
             if ((demand - supply) > 0)
            {       
                global_market.goods_list[i].price += global_market.goods_list[i].price * priceSensitivity;
            }
            else
            {   
                global_market.goods_list[i].price -= global_market.goods_list[i].price * priceSensitivity;
            }

            //reset supply and demand beggening of the month
            global_market.goods_list[i].supply = 0;
            global_market.goods_list[i].demand = 0;
            Debug.Log("supply:" + supply);
            Debug.Log("demand:" + demand);
            Debug.Log($"wood price is now : {global_market.goods_list[i].price}, it have change of {global_market.goods_list[i].price - old_price}$");
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