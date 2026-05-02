using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MarketTransactionsObj;

public class HandleMarketBuy
{
    public static List<MarketBuyResponse> ProcessMarketRequest(List<MarketBuyRequest> requestList, List<Market> marketList)
    {
        List<MarketBuyResponse> marketResponses = new List<MarketBuyResponse>();

        foreach (MarketBuyRequest request in requestList) 
        {
            //construction of response object
            MarketBuyResponse response = new MarketBuyResponse();

            response.id = request.id;
            response.goodsBought = new List<GoodRequest>();

            int market_id = request.marketId;
            Market _market = marketList[market_id];
            int cashAmount = request.cashAmount;

            for(int j = 0; j < request.GoodRequests.Count; j++)
            {
                GoodRequest goodResponse = new GoodRequest();

                int goodId = request.GoodRequests[j].goodId;

                goodResponse.goodId = goodId;

                MarketGood Marketgood = _market.goods_list
                .Where(good => good.good.id == goodId).FirstOrDefault();


                int amountWanted = request.GoodRequests[j].amount;
                //TODO Debug.Log($"price : {Marketgood.price}, ammount wanted : {amountWanted} ");
                int totalCost = Marketgood.price * amountWanted;
                if (cashAmount == 0)
                {
                    break;
                   
                }


                if (totalCost <= cashAmount)
                {
                    // Can afford the full amount
                    goodResponse.amount = amountWanted;
                    cashAmount -= totalCost;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("full amount");
                    Debug.Log("cash left :" + response.cashLeft);

                    Marketgood.demand += amountWanted;
                    Marketgood.stockpile -= amountWanted;

                }
                else
                {
                    // Can only afford partial amount
                    int amountAffordable = cashAmount / Marketgood.price;
                    goodResponse.amount = amountAffordable;
                    cashAmount = 0;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("partial amount");

                    Marketgood.demand += amountWanted + 1;
                    Marketgood.stockpile -= amountAffordable;

                    break; // Exit loop early, no cash left
                }
            }
            response.cashLeft = cashAmount;
            marketResponses.Add(response);
        }
        return marketResponses;
    }    
 }

