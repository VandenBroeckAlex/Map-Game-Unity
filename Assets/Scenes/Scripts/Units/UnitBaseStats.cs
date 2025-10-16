using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UnitStats
{
    public FloatCurentMax organisation;
    public FloatCurentMax moral;

    public FloatCurentMax manPower;
    public FloatCurentMax officer;
    public List<PopGood> supply;
    //public List<> supply;
    //public Weight weight;
    // public float speed
    //public float coalConsumption;
    //public float armor;
    //public float softness; (%)

    //public float airDefense;
    public float softAttack;
    //public float hardAttack;
    // public float melee
    //public int  combatWidth;
    // public FCurentMax entrenchement
    //public level
    //public experience


    public float CurentSoftAttack()
    {
        if (manPower.max == 0)
            return 0;

        return softAttack / ((manPower.current / manPower.max) * 100f);
    }

    public int PercentManPower()
    {
        if (manPower.max == 0)
            return 0;

        return Mathf.RoundToInt((manPower.current / manPower.max) * 100f);
    }
}
