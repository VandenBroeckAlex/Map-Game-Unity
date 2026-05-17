using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static MarketTransactionsObj;

public class HandleMarketBuy
{
    public static List<MarketBuyResponse> ProcessMarketBuyRequest(DataRegistery registery)
    {
        List<MarketBuyRequest> requestList = registery.marketBuyRequests;
        List<Market> marketList = registery.marketList;
        List<MarketBuyResponse> marketResponses = new List<MarketBuyResponse>();

        foreach (MarketBuyRequest request in requestList) 
        {
            //construction of response object
            MarketBuyResponse response = new MarketBuyResponse();

            response.id = request.id;
            response.goodsBought = new List<GoodRequest>();
            response.domain = request.domain;
            int market_id = request.marketId;
            Market _market = marketList[market_id];
            int availableCashAmount = request.cashAmount;
            int totalCost = 0;
            for (int j = 0; j < request.GoodRequests.Count; j++)
            {
                if (availableCashAmount == 0)
                {
                    break;
                }

                GoodRequest goodResponse = new GoodRequest();
                int goodId = request.GoodRequests[j].goodId;
                goodResponse.goodId = goodId;

                MarketGood Marketgood = _market.goods_list
                .Where(good => good.good.id == goodId).FirstOrDefault();

                int amountWanted = request.GoodRequests[j].amount;
                int maxcost = Marketgood.price * amountWanted;


                if (maxcost  <= availableCashAmount)
                {
                    // Can afford the full amount
                    goodResponse.amount = amountWanted;
                    availableCashAmount -=  maxcost;
                    totalCost -= maxcost;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("full amount");
                    Debug.Log("cash left :" + response.cost);

                    Marketgood.demand += amountWanted;
                    Marketgood.stockpile -= amountWanted;

                }
                else
                {
                    // Can only afford partial amount
                    int amountAffordable = (availableCashAmount ) / Marketgood.price;
                    goodResponse.amount = amountAffordable;
                    availableCashAmount = -request.cashAmount;
                    totalCost -= -request.cashAmount;
                    response.goodsBought.Add(goodResponse);
                    Debug.Log("partial amount");

                    Marketgood.demand += amountWanted + 1;
                    Marketgood.stockpile -= amountAffordable;

                    break; // Exit loop early, no cash left
                }
            }
            response.cost = totalCost;
            marketResponses.Add(response);
        }
        return marketResponses;
    }

    public static void ProcessMarketBuyReponse(DataRegistery _registery)
    {
        foreach (MarketBuyResponse response in _registery.marketBuyResponseBuffer)
        {
            switch (response.domain)
            {
                case RequestDomain.Population:

                    if (_registery.PopulationDict.TryGetValue(response.id, out Pop pop))
                    {
                        pop.AddGoods(response.goodsBought);
                        pop.AddCash(response.cost);
                    }
                    else
                    {
                        //TODO throw invalid popId error
                    }
                    break;

                case RequestDomain.Building:
                    //getbuilding
                    WorkplaceInstance building = _registery.buildings.Where(building => building.GetWorkplaceId() == response.id).FirstOrDefault();
                    if (building != null)
                    {
                        building.AddCash(response.cost);
                        foreach(GoodRequest gr in response.goodsBought)
                        {
                            WorkplaceTemplate template = _registery.workplaceTemplate[building.TemplateId];
                            building.AddGood(gr.goodId,gr.amount,template);
                        }
                    }
                    else
                    {
                        //TODO throw invalid building error
                    }

                    //add cash
                    break;

                case RequestDomain.Country:
                    //getcountry
                    if (_registery.countryDict.TryGetValue(response.id, out Country country))
                    {
                        country.ReceiveCash(response.cost);
                        //Add to stockpile
                    }
                    else
                    {
                        //TODO throw invalid country id error
                    }
                    break;
            }

        }
    }
    
 }

