using System;
using System.Collections.Generic;

public class HandleGrowth
{
    public List<Pop> PopGrowth(List<Pop> populationList, float base_growth_rate, float countryGrowthModdifier, float provinceGrowthModdifier)
    {
        for (int i = 0; i < populationList.Count; i++)
        {
            // change pop MaxNeed at the same time (popsize / 1000) * MaxNeed
            if (populationList[i].HaveBasicNeed())
            {
                float growRate = base_growth_rate + (1 * countryGrowthModdifier) + provinceGrowthModdifier;
                int newPopulation = (int)Math.Round(populationList[i].size * growRate);
                populationList[i].size += newPopulation;
            }
        }
        return populationList;
    }
}