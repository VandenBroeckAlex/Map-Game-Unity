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
                goodRequest.amount = good.MaxNeed - good.Stockpile;
                if (goodRequest.amount > 0)
                {
                    goodRequest.goodId = good.Good_id;
                    goodRequestsList.Add(goodRequest);
                }
            }
            request.GoodRequest = goodRequestsList;
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
                   GoodRequirement goodReq = pop.GoodList.Where(g => g.Good_id == goodBought.goodId).FirstOrDefault();
                   goodReq.Stockpile += goodBought.amount;
                }
            }
        }
        return _popList;
    }
}