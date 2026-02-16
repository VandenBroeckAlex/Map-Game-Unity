using static MarketTransactionsObj;
using System.Collections.Generic;
using System.Linq;

public static class HandlePopBuy
{
     public static List<PopBuyRequest> PopulationBuyRequest(List<Pop> _popList)
    {
        List <PopBuyRequest> result = new List<PopBuyRequest> ();
        foreach (var pop in _popList)
        {
            PopBuyRequest request = new PopBuyRequest();
            List<GoodRequest> goodRequestsList = new List<GoodRequest>();
            request.popId = pop.id;
            request.cashAmmount = pop.cashAmount;
            request.marketId = pop.countryID;
            foreach (GoodRequirement good in pop.GoodList)
            {               
                GoodRequest goodRequest = new GoodRequest();
                goodRequest.goodId = good.Good_id;
                goodRequest.amount = good.MaxNeed - good.Stockpile;
                goodRequestsList.Add(goodRequest);
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
        
        }

        return _popList;
    }
}