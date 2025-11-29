using System.Collections.Generic;

public class CountryStats
{
   

    public ModifierContainer modifiers = new ModifierContainer();



    public float GetPopulationGrowth()
    {       
        return  modifiers.GetModifierValue("pop_growth"); 
    }
}
