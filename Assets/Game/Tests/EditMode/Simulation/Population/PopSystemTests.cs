using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

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
    public void HandlePopBuyTest()
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

}
