/*
Compute workplace need

Create Request buffer

Apply it to BuyRequestbuffer  in registery
 */
using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;

public class HandleWorkplaceBuy
{
    public static void ProcessWorkplaceBuy(DataRegistery registery)
    {
        List<MarketBuyRequest> result = new List<MarketBuyRequest> ();
        
        foreach (WorkplaceInstance building in registery.workplacesInstances)
        {
            registery.workplaceTemplate.TryGetValue(building.TemplateId, out WorkplaceTemplate template);
            List<GoodRequest> requestList = new List<GoodRequest> ();

            foreach(GoodRequirement gr in building.maintenanceGoodsStockpile)
            {
                int maxAmount = building.size * template.GetMaxGoodAmountById(gr.good_id);

                int amoutLeftToBuy = maxAmount - gr.stockpile;

                if(amoutLeftToBuy > 0)
                {
                    GoodRequest request = new GoodRequest ();
                    request.goodId = gr.good_id;
                    request.amount = amoutLeftToBuy;
                    requestList.Add (request);
                }
                else
                {
                    //TODO HANDLE ERROR
                }
            }
            MarketBuyRequest mbr = new MarketBuyRequest ();
            mbr.marketId = building.GetMarketId();
            mbr.id = building.GetWorkplaceId();
            mbr.GoodRequests = requestList;
            mbr.cashAmount = building.GetCash();
           result.Add (mbr);
        }
        registery.marketBuyRequests.Concat(result);
    }
}
