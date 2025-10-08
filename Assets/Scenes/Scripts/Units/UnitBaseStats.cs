using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UnitStats
{
    public FCurentMax strength;
    public FCurentMax organisation;
    public FCurentMax moral;

    public FCurentMax ManPower;
    public FCurentMax Officer;

    //public Supply supply;
    //public Weight weight;

    //public float coalConsumption;
    //public float armor;
    //public float softness; (%)

    //public float airDefense;
    public float softAttack;
    //public float hardAttack;

    //public int  combatWidth;

    public  float CurentSoftAttack()
    {
        if (ManPower.Max == 0)
            return 0;

        return softAttack / ((ManPower.Current / ManPower.Max) * 100f);
    }

    public int PercentManPower()
    {
        if (ManPower.Max == 0)
            return 0;

        return Mathf.RoundToInt((ManPower.Current / ManPower.Max) * 100f);
    }
}
