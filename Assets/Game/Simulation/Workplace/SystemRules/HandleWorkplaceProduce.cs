using static MarketTransactionsObj;
/*
Compute workplace production

Create Request buffer

Apply it to SellRequestbuffer  in registery
 */
public static class HandleWorkplaceProduce
{
    public static void ProcessProduction(DataRegistery registery)
    {
        foreach (WorkplaceInstance building in registery.buildings)
        {
            if (building.canProduce)
            {
                //get template
                registery.workplaceTemplate.TryGetValue(building.TemplateId, out WorkplaceTemplate definition);
                foreach (ProductionEffect output in definition.outputs)
                {
                    int ammount = (int)(output.baseAmount * (building.CalculateEmploymentRatio(definition)));// input + effi input



                    // Route the output to the correct "Sink"
                    switch (output.type)
                    {
                        case OutputDomain.Market:
                            // Push to the Market's "Supply" buffer for this tick
                            GoodSellRequest gsr = new GoodSellRequest();
                            gsr.amountsell = ammount;
                            gsr.goodId = output.targetId;
                            gsr.domain = RequestDomain.Building;
                            MarketSellRequest request = new MarketSellRequest();
                            request.id = building.GetWorkplaceId();
                            request.marketId = building.GetMarketId();
                            request.goodSell = gsr;
                            //TODO add in buffer
                            registery.marketSellRequestBuffer.Add(request);
                            break;

                        case OutputDomain.Country:
                            //// Get the owner of the province this building is in
                            //int ownerId = registry.GetProvince(building.ProvinceId).OwnerId;
                            //registry.GetCountry(ownerId).AddResource(output.ResourceId, finalAmount);
                            break;

                        case OutputDomain.Province:
                            //registry.GetProvince(building.ProvinceId).AddInfrastructure(finalAmount);
                            break;
                        case OutputDomain.Tile:
                            break;
                    }
                }
            }
        }
    }
}
