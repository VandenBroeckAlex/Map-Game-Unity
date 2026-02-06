
using System.Collections.Generic;
using System.Linq;

public class HandleGet
{
    public List<Pop> SelectPopulationByCountry(int countryId, List<Pop> populationList)
    {
        return populationList.Where(p => p.countryID == countryId).ToList();
    }
    public List<Pop> SelectPopulationByProvince(int provinceID, List<Pop> populationList)
    {
        List<Pop> selectedPops = new();
        for (int i = 0; i < populationList.Count; i++)
        {
            if (populationList[i].provinceId == provinceID)
            {
                selectedPops.Add(populationList[i]);
            }
        }
        return selectedPops;
    }
}
