using System.Collections.Generic;

public class MarketBuyRequest
{
    public int Id;
    public int marketId;
    public List<GoodBuyRequest> GoodRequest;
    public int cashAmount;
}