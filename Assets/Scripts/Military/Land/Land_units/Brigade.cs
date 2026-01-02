using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brigade 
{
    public string name;
    public int id;
    [SerializeField]
    public List<Bataillon> bataillons;
    public List<GoodRequirement> supply;
    public IntCurentMax Manpower;
    

    public IntCurentMax GetManPower()
    {
        IntCurentMax mp = new IntCurentMax(0,0);
        foreach(Bataillon bataillon in bataillons)
        {
            mp.current += bataillon.manPower.current;
            mp.max += bataillon.manPower.max;
        }
        return mp;
    }
    public FloatCurentMax GetAverageOrganisation()
    {
        FloatCurentMax orga  = new FloatCurentMax(0,0);
        foreach(Bataillon bataillon in bataillons)
        {
            orga.current += bataillon.organisation.current;
            orga.max += bataillon.organisation.max;
        }

        return orga;
    }

    //All bataillon must be routed
    public bool IsRouted()
    {
        foreach(var bataillon in bataillons)
        {
            if(bataillon.isRouted == false)
            {
                return false;
            }
        }
        return true;
    }
}
