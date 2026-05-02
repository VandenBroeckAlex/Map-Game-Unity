using System.Collections.Generic;
using UnityEngine;

public class MarketTransactionsObj
{

    public struct MarketBuyRequest : IEvent 
    {
        public int marketId;
        public int id;
        public List<GoodRequest> GoodRequests;
        public int cashAmount;
    }

    public struct MarketSellRequest : IEvent
    {
        public int Id;
        public int marketId;
        public GoodSellRequest goodSell;
        public MarketSellRequest(int id, int marketId, GoodSellRequest goodSell)
        {
            Id = id;
            this.marketId = marketId;
            this.goodSell = goodSell;
        }
    }

    public struct GoodRequest : IEvent
    {
        public int goodId;
        public int amount;
        public GoodRequest(int goodId, int amountWanted)
        {
            this.goodId = goodId;
            amount = amountWanted;
        }
    }

    public struct GoodSellRequest : IEvent
    {
        public int goodId;
        public int amountsell;
        public GoodSellRequest(int goodId, int amountsell)
        {
            this.goodId = goodId;
            this.amountsell = amountsell;
        }
    }

    public struct MarketSellResponse : IEvent
    {
        public int id;
        public int cashRecived;
    }


    public struct MarketBuyResponse : IEvent
    {
        public int id;
        public List<GoodRequest> goodsBought;
        public int cashLeft;
    }
}
