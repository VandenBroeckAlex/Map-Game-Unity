using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using static Goods;



// If game creation (not loading a save) 

[System.Serializable]
public class Market_good
{
    public int id;
    public Good good;
    public float supply;
    public float demand;
    public float price;
}

[System.Serializable]
public class Market
{
    public int id;
    public string name = "test";
    public float CashAmount = 0;
    public List<Market_good> goods_list = new();



}

public class MarketManager : MonoBehaviour
{
    [SerializeField] public Market global_market = new Market();

    private void Start()
    {
        Goods.Good good = ScriptableObject.CreateInstance<Goods.Good>();

        good.basePrice = 1;
        good.goodName = "wood";
        good.weight = 0;
        good.type = GoodType.Raw;

        global_market.goods_list.Add(new Market_good
        {
            id = 1,
            good = good,
            supply = 1000f,
            demand = 0f,
            price = 1f
        });
    }

    private void On_Pop_Buy(int marketId, float moneyAmount, Dictionary<int, float> PopStockpile, Dictionary<int, float> PopMaxNeeds)
    {
        // Market curentMarket = marketList.FirstOrDefault(market => market.id == marketId);

    }
}

/*
public class MarketRequest {
    public int popId;
    public int marketId;
    public List<GoodRequest> goods;
    public float cashAmount;
}

public class GoodRequest {
    public int goodId;
    public float amountWanted;
}

public class MarketResponse {
    public int popId; // So you know who to route this back to
    public List<GoodResponse> goodsBought;
    public float cashLeft;
}

public class GoodResponse {
    public int goodId;
    public float amountBought;
}
 */


/*
1 Production Phase:

    Buildings produce goods.

    Output goes into the global (or local) market.

    Needs Assessment Phase:

    Pops calculate their demand based on size.

    Wealth limits what they can actually buy.

2 Trade Phase:

    Goods are matched with demand.

    Prices adjust.

    Wealth is transferred from consumers to producers.

4 Population Update Phase:

    Unmet needs lower happiness.

    Overconsumption can raise class mobility or birthrate.

    Wealth growth/shrinkage is tracked.
 */