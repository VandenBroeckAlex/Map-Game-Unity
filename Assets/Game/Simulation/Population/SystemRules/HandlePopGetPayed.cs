
using System.Collections.Generic;
public static class HandlePopGetPayed
{
    public static DataRegistery PopGetPayed(DataRegistery _registery, List<IdNum> order)
    {
        foreach (IdNum iN in order) 
        {
            _registery.PopulationDict.TryGetValue(iN.id, out Pop pop);

            if(pop != null)
            {
                pop.cashAmount += iN.num;
            }
        }
        return _registery;
    }
}
