using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static MarketTransactionsObj;

public class MarketSystemTest
{
    private MarketGood CreateMarketGood(int id,Good good,int price, int stockpile)
    {
        MarketGood mg = new MarketGood();
        mg.id = id;
        mg.good = good;
        mg.price = price;
        mg.stockpile = stockpile;

        return mg;
    }
    private MarketBuyRequest CreateMarketRequest(int id, int marketID,int cashAmmount, List<GoodRequest> gr)
    {
        MarketBuyRequest request = new MarketBuyRequest();
        request.id = 0;
        request.marketId = 0;
        request.cashAmount = 1000;
        request.GoodRequests = gr;

        return request;
    }

    [Test]
    public void HandleMarketBuy_ProcessMarketRequest_test()
    {
        //create list of request
        List<MarketBuyRequest> requestsList = new List<MarketBuyRequest>();



        List<GoodRequest> grList = new List<GoodRequest>();
        GoodRequest gr = new GoodRequest();
        gr.amount = 5;
        gr.goodId = 0;


        grList.Add(gr);
        MarketBuyRequest mbr = CreateMarketRequest(0, 0, 1000, grList);

        requestsList.Add(mbr);

        //create Market with easilly testable values
        List<Market> marketList = new List<Market>();
        Market market = new Market();
        Good good = new Good();
        good.id = 0;

        MarketGood mg = CreateMarketGood(0, good, 200, 100);
      
        market.goods_list = new List<MarketGood>();
        market.goods_list.Add(mg);

        marketList.Add(market);

        DataRegistery _registery = new DataRegistery();
        _registery.marketList = marketList;
        _registery.marketBuyRequests = requestsList;


        //test returned value
        List<MarketBuyResponse> response = HandleMarketBuy.ProcessMarketBuyRequest(_registery);
        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Count);
        Assert.AreEqual(gr.amount * -mg.price, response[0].cost);
  
        Assert.AreEqual(5, response[0].goodsBought[0].amount);
    }


    [Test]
    public void HandleMarketBuy_ProcessMarketMultipleRequest_test()
    {
        List<MarketBuyRequest> requestsList = new List<MarketBuyRequest>();

        List<GoodRequest> grList = new List<GoodRequest>();
        GoodRequest gr = new GoodRequest();
        gr.amount = 5;
        gr.goodId = 0;


        grList.Add(gr);
        MarketBuyRequest mbr = CreateMarketRequest(0, 0, 1000, grList);
        MarketBuyRequest mbr2 = CreateMarketRequest(1, 0, 1000, grList);

        requestsList.Add(mbr);
        requestsList.Add(mbr2);

        //create Market with easilly testable values
        List<Market> marketList = new List<Market>();
        Market market = new Market();
        Good good = new Good();
        good.id = 0;

        MarketGood mg = CreateMarketGood(0, good, 200, 100);

        market.goods_list = new List<MarketGood>();
        market.goods_list.Add(mg);

        marketList.Add(market);

        DataRegistery _registery = new DataRegistery();
        _registery.marketList = marketList;
        _registery.marketBuyRequests = requestsList;
        List<MarketBuyResponse> response = HandleMarketBuy.ProcessMarketBuyRequest(_registery);

        Assert.IsNotNull(response);
        Assert.AreEqual(2, response.Count);
    }
}
