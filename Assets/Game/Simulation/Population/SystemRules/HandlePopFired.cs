/*
Recive data from workplace
pass info into pop
 */

using System.Collections.Generic;

public  class PopFiredFromWorkplace
{
    public  void FiredFromWorkplace(List<PopFired> data, DataRegistery _registery)
    {
        foreach(PopFired pf in data)
        {
            Dictionary<int, Pop> popDictionary = _registery.PopulationDict;

            if(popDictionary.TryGetValue(pf.popId, out Pop pop))
            {
                pop.FiredFromWorkplace(pf.workplaceId, pf.amount);
            }
            else
            {
                //raise pop not found error
            }
        }
    }
}