using UnityEngine;
using NUnit.Framework;
public class MarketGoodTest
{
    [Test]
    public void Test_GoodMarketPriceVolatility()
    {
        Good good = new Good();
        good.basePrice = 100;
        good.id = 0;
        MarketGood marketGood = new MarketGood();
        marketGood.good = good;
        marketGood.price = 100;
        marketGood.supply = 50;
        marketGood.demand = 200;

        marketGood.RecordGoodHistory();

        Assert.AreEqual(marketGood.GetPriceHistory()[0], 100);
        Assert.AreEqual(marketGood.GetDemandHistory()[0], 200);
        Assert.AreEqual(marketGood.GetSupplyHistory()[0], 50);
    }

}
