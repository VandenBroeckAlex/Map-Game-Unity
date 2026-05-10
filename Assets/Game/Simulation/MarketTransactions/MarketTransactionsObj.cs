using System.Collections.Generic;
using UnityEngine;

public class MarketTransactionsObj
{
    public enum RequestDomain
    {
        Country,
        Building,
        Population
    }

    public struct MarketBuyRequest : IEvent 
    {
        public int marketId;
        public int id;
        public RequestDomain domain;
        public List<GoodRequest> GoodRequests;
        public int cashAmount;
    }

    public struct MarketSellRequest : IEvent
    {
        public int id;
        public int marketId;
        public RequestDomain domain;
        public GoodSellRequest goodSell;
        public MarketSellRequest(int id, int marketId, GoodSellRequest goodSell, RequestDomain domain)
        {
            this.id = id;
            this.marketId = marketId;
            this.goodSell = goodSell;
            this.domain = domain;
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
        public RequestDomain domain;
        public GoodSellRequest(int goodId, int amountsell, RequestDomain domain)
        {
            this.goodId = goodId;
            this.amountsell = amountsell;
            this.domain = domain;
        }
    }

    public struct MarketSellResponse : IEvent
    {
        public int id;
        public RequestDomain domain;
        public int cashRecived;
    }


    public struct MarketBuyResponse : IEvent
    {
        public int id;
        public RequestDomain domain;
        public List<GoodRequest> goodsBought;
        public int cost;
    }
}
