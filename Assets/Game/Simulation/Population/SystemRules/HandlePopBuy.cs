using static MarketTransactionsObj;
using System.Collections.Generic;
using System.Linq;

public static class HandlePopBuy
{
     public static List<MarketBuyRequest> PopulationBuyRequest(List<Pop> _popList)
    {
        List <MarketBuyRequest> result = new List<MarketBuyRequest> ();
        foreach (var pop in _popList)
        {
            MarketBuyRequest request = new MarketBuyRequest();
            List<GoodRequest> goodRequestsList = new List<GoodRequest>();
            request.id = pop.id;
            request.cashAmount = pop.cashAmount;
            request.marketId = pop.countryID;
            foreach (GoodRequirement good in pop.GoodList)
            {               
                GoodRequest goodRequest = new GoodRequest();
                goodRequest.amount = good.maxNeed - good.stockpile;
                if (goodRequest.amount > 0)
                {
                    goodRequest.goodId = good.good_id;
                    goodRequestsList.Add(goodRequest);
                }
            }
            request.GoodRequests = goodRequestsList;
            result.Add(request);
        }
        return result;
    }
     
    public static List<Pop> PopulationBuyProcess(List<Pop> _popList, List<MarketBuyResponse> response)
    {
        foreach (MarketBuyResponse marketBuyResponse in response) 
        { 
            Pop pop = _popList.Where(p => p.id == marketBuyResponse.id).FirstOrDefault();
            
            if(pop != null)
            {
                pop.cashAmount = marketBuyResponse.cashLeft;
                foreach(GoodRequest goodBought in marketBuyResponse.goodsBought)
                {
                   GoodRequirement goodReq = pop.GoodList.Where(g => g.good_id == goodBought.goodId).FirstOrDefault();
                   goodReq.stockpile += goodBought.amount;
                }
            }
        }
        return _popList;
    }
}