using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brigade 
{
    public string name;
    [SerializeField]
    public List<Bataillon> bataillons;
    public List<PopGood> supply;
    public IntcurentMax Manpower;

    public IntcurentMax GetManPower()
    {
        IntcurentMax mp = new IntcurentMax(0,0);
        foreach(Bataillon bataillon in bataillons)
        {
            mp.current += bataillon.manPower.current;
            mp.max += bataillon.manPower.max;
        }
        return mp;
    }
}
