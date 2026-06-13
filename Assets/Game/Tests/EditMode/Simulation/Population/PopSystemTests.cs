using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using static MarketTransactionsObj;

public class PopSystemTests
{
    private static Pop CreatePop()
    {
        GoodRequirement gr = new GoodRequirement(1, 0, 500);
        List<GoodRequirement> goodReq = new List<GoodRequirement>();
        goodReq.Add(gr);
        Pop pop = new Pop(0, 1000, 1, 1, 1, 1, 100, goodReq, new List<IdNum>());
        pop.countryID = 1;
        return pop;
    }
    [Test]
    public void PopSystem_HandlePopBuy_returnTrue()
    {
        HandlePopBuy popBuySystem = new HandlePopBuy();
        Dictionary<int,Pop> poplist = new Dictionary<int, Pop>();
        poplist.Add(1,CreatePop());

        List<MarketTransactionsObj.MarketBuyRequest> result = popBuySystem.PopulationBuyRequest(poplist);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(result[0].GoodRequests.Count, 1);
        Assert.AreEqual(result[0].GoodRequests[0].goodId , 1);
        Assert.AreEqual(result[0].GoodRequests[0].amount, 500);
        Assert.AreEqual(result[0].cashAmount, 100);
        Assert.AreEqual(result[0].marketId, 1);
    }
    [Test]
    public void PopSystem_HandlePopBuy_02_returnTrue()
    {
        HandlePopBuy popBuySystem = new HandlePopBuy();
        Dictionary<int, Pop> poplist = new Dictionary<int, Pop>();
        poplist.Add(0, CreatePop());
        poplist.Add(1,CreatePop());
        poplist.Add(2,CreatePop());
        poplist.Add(3,CreatePop());
  

        List<MarketBuyRequest> result = popBuySystem.PopulationBuyRequest(poplist);
        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count);
    }
    [Test]
    public void PopSystem_HandlePopBuy_PopulationBuyProcess()
    {
        List<Pop> poplist = new List<Pop>();
        poplist.Add(CreatePop());
        List<MarketBuyResponse> marketResponses = new List<MarketTransactionsObj.MarketBuyResponse>();

        MarketBuyResponse marketResponse = new MarketBuyResponse();
        marketResponse.cost = 0;
        marketResponse.id = 0;
        marketResponse.goodsBought =  new List<GoodRequest>();
        GoodRequest goodrequest = new GoodRequest();
        goodrequest.amount = 500;
        goodrequest.goodId = 1;
        marketResponse.goodsBought.Add(goodrequest);
        marketResponses.Add(marketResponse);

        poplist = HandlePopBuy.PopulationBuyProcess(poplist, marketResponses);
    
        Assert.IsNotNull(poplist);
        Assert.AreEqual(0,poplist[0].cashAmount);
        Assert.AreEqual(poplist[0].GoodList[0].stockpile, 500);
    }

    [Test]
    public void PopSystem_HandlePopResetStockpile()
    {
        HandlePopBuy popBuySystem = new HandlePopBuy();
        HandlePopulationResetStockpile resetStockpileSystem = new HandlePopulationResetStockpile();
        Dictionary<int, Pop> popList = new Dictionary<int, Pop>();
        Pop pop = CreatePop();
        pop.GoodList[0].stockpile = 500;

        popList.Add(0,pop);
        popList.Add(1,CreatePop());
        popList = resetStockpileSystem.ResetStockpile(popList);

        Assert.IsNotNull(popList);
        Assert.AreEqual(2, popList.Count);
        Assert.AreEqual(0, popList[0].GoodList[0].stockpile);
    }
}
