using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static MarketTransactionsObj;

public class MarketSystemTest
{
    [Test]
    public void HandleMarketBuy_ProcessMarketRequest_test()
    {
        //create list of request
        List<MarketBuyRequest> requestsList = new List<MarketBuyRequest>();

        MarketBuyRequest request = new MarketBuyRequest();
        request.marketId = 0;
        request.cashAmount = 1000;

        request.GoodRequest = new List<GoodRequest>();

        GoodRequest gr = new GoodRequest();
        gr.amount = 5;
        gr.goodId = 0;

        request.GoodRequest.Add(gr);
        requestsList.Add(request);


        //create Market with easilly testable values
        List<Market> marketList = new List<Market>();
        Market market = new Market();
        MarketGood mg = new MarketGood();
        Good good = new Good();
        good.id = 0;

        mg.id = 0;
        mg.good = good;
        mg.price = 200;
        mg.stockpile = 100;
        market.goods_list = new List<MarketGood>();
        market.goods_list.Add(mg);

        marketList.Add(market);

        //test returned value
        List<MarketBuyResponse> response = HandleMarketBuy.ProcessMarketRequest(requestsList, marketList);
        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Count);
        Assert.AreEqual(0, response[0].cashLeft);
  
        Assert.AreEqual(5, response[0].goodsBought[0].amount);
    }
}
