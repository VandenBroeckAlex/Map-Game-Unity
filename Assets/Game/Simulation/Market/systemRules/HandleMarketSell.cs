using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;

public class HandleMarketSell
{
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
