using System.Linq;
using UnityEngine;
using static MarketTransactionsObj;

public static class MarketSystem
{
    public static void ProcessSellResponse(DataRegistery registery)
    {
        foreach (var request in registery.marketSellResponseBuffer)
        {
           
                switch (request.domain)
                {
                    case RequestDomain.Population:
   
                        if(registery.PopulationDict.TryGetValue(request.id, out Pop pop))
                        {
                            pop.AddCash(request.cashRecived);
                        }
                        else
                        {
                            //TODO throw invalid popId error
                        }
                            break;

                    case RequestDomain.Building:
                        //getbuilding
                        Building building = registery.buildings.Where(building => building.GetWorkplaceId() == request.id).FirstOrDefault();
                        if(building != null)
                        {
                            building.AddCash(request.cashRecived);
                        }
                        else
                        {
                            //TODO throw invalid building error
                        }

                        //add cash
                        break;

                    case RequestDomain.Country:
                    //getcountry
                    if (registery.countryDict.TryGetValue(request.id, out Country country))
                    {
                        country.ReceiveCash(request.cashRecived);
                    }
                    else
                    {
                        //TODO throw invalid country id error
                    }
                        break;
                }

            }
        }
}

