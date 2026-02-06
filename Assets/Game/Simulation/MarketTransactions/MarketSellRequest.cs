public class MarketSellRequest
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