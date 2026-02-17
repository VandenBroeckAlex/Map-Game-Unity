using System;
using System.Collections.Generic;

public static class HandlePopulationResetStockpile
{
    public static List<Pop> ResetStockpile(List<Pop> popList)
    {
        for (int i = 0; i < popList.Count; i++)
        {
            for (int j = 0; j < popList[i].GoodList.Count; j++)
            {
                popList[i].GoodList[j].Stockpile = 0;
            }
        }
        return popList;
    }
}