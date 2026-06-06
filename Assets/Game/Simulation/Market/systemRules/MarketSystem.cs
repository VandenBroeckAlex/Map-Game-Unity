using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;

public static class MarketSystem
{
    public static void ProcessSellResponse(DataRegistery registery)
    {
        foreach (var request in registery.marketSellResponseBuffer)
        {
           
                switch (request.domain)
                {
                    case RequestDomain.Population:
   
                        if(registery.PopulationDict.TryGetValue(request.id, out Pop pop))
                        {
                            pop.AddCash(request.cashRecived);
                        }
                        else
                        {
                            //TODO throw invalid popId error
                        }
                            break;

                    case RequestDomain.Building:
                        //getbuilding
                        WorkplaceInstance building = registery.buildings.Where(building => building.GetWorkplaceId() == request.id).FirstOrDefault();
                        if(building != null)
                        {
                            building.UpdateCash(request.cashRecived);
                        }
                        else
                        {
                            //TODO throw invalid building error
                        }

                        //add cash
                        break;

                    case RequestDomain.Country:
                    //getcountry
                    if (registery.countryDict.TryGetValue(request.id, out Country country))
                    {
                        country.ReceiveCash(request.cashRecived);
                    }
                    else
                    {
                        //TODO throw invalid country id error
                    }
                        break;
                }

            }
        }

    public static List<MarketSellResponse> ProcessMarketSellRequest(List<MarketSellRequest> requestList, List<Market> marketList)
    {
        List<MarketSellResponse> marketSellResponses = new List<MarketSellResponse>();

        foreach (MarketSellRequest request in requestList)
        {
            MarketSellResponse response = new MarketSellResponse();
            response.id = request.id;
            response.domain = request.domain;

            //get market
            Market market = marketList.Where(market => market.id == request.marketId).FirstOrDefault();

            if (market != null)
            {
                MarketGood good = market.goods_list.Where(good => good.id == request.goodSell.goodId).FirstOrDefault();
                if (good != null)
                {
                    int ammount = request.goodSell.amountsell;
                    int cash = ammount * good.price;

                    //Market creat the cash / keep track of how much was created
                    market.TrackMoneyCreation(cash);

                    good.supply += ammount;
                    good.stockpile += ammount;
                    response.cashRecived = cash;
                }
                else
                {
                    //TODO raise invalid goodId Error
                }
            }
            else
            {
                //TODO raise invalid MarketId Error
            }
            marketSellResponses.Add(response);
        }
        return marketSellResponses;
    }
}

