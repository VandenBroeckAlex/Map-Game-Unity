using System.Collections.Generic;

public  class HandlePopulationResetStockpile
{
    public Dictionary<int, Pop> ResetStockpile(Dictionary<int,Pop> popList)
    {
        for (int i = 0; i < popList.Count; i++)
        {
            for (int j = 0; j < popList[i].GoodList.Count; j++)
            {
                popList[i].GoodList[j].stockpile = 0;
            }
        }
        return popList;
    }
}