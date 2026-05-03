using static MarketTransactionsObj;

public class ProductionSystem
{
    public void ProcessProduction(DataRegistery registry)
    {
        foreach (var building in registry.buildings)
        {
            if (building.canProduce)
            {
                int ammount = building.Produce();

                // Route the output to the correct "Sink"
                switch (building.outputDomain)
                {
                    case OutputDomain.Market:
                        // Push to the Market's "Supply" buffer for this tick
                        GoodSellRequest gsr = new GoodSellRequest();
                        gsr.amountsell = ammount;
                        gsr.goodId = building.production.producedGoodId;

                        MarketSellRequest request = new MarketSellRequest();
                        request.Id = building.GetWorkplaceId();
                        request.marketId = building.GetMarketId();
                        request.goodSell = gsr;
                        //TODO add in buffer
                        registry.marketSellRequestBuffer.Add(request);
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
