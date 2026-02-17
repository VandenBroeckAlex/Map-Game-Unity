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
        Pop pop = new Pop(0, 1000, 1, 1, 1, 1, 100, goodReq);
        pop.countryID = 1;
        return pop;
    }
    [Test]
    public void PopSystem_HandlePopBuy_returnTrue()
    {
        List<Pop> poplist = new List<Pop>();
        poplist.Add(CreatePop());

        List<MarketTransactionsObj.PopBuyRequest> result = HandlePopBuy.PopulationBuyRequest(poplist);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(result[0].GoodRequest.Count, 1);
        Assert.AreEqual(result[0].GoodRequest[0].goodId , 1);
        Assert.AreEqual(result[0].GoodRequest[0].amount, 500);
        Assert.AreEqual(result[0].cashAmmount, 100);
        Assert.AreEqual(result[0].marketId, 1);
    }
    [Test]
    public void PopSystem_HandlePopBuy_02_returnTrue()
    {
        List<Pop> poplist = new List<Pop>();
        poplist.Add(CreatePop());
        poplist.Add(CreatePop());
        poplist.Add(CreatePop());
        poplist.Add(CreatePop());

        List<PopBuyRequest> result = HandlePopBuy.PopulationBuyRequest(poplist);
        Assert.IsNotNull(result);
        Assert.AreEqual(4, result.Count);
    }
    [Test]
    public void PopSystem_HandlePopBuy_PopulationBuyProcess()
    {
        List<Pop> poplist = new List<Pop>();
        poplist.Add(CreatePop());
        Debug.Log(poplist[0].id);
        List<MarketBuyResponse> marketResponses = new List<MarketTransactionsObj.MarketBuyResponse>();

        MarketBuyResponse marketResponse = new MarketBuyResponse();
        marketResponse.cashLeft = 0;
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
        Assert.AreEqual(poplist[0].GoodList[0].Stockpile, 500);
    }

}
