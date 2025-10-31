using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Bataillon 
{
    [SerializeField]
    public int id;
    public FloatCurentMax organisation;
    public FloatCurentMax moral;
    public IntcurentMax manPower;
    public IntcurentMax officer;
    public List<PopGood> supply;
    public bool isSupport;
    //public List<> supply;
    //public Weight weight;
    // public float speed
    //public float coalConsumption;
    //public float armor;
    //public float softness; (%)
    public int range; // Should be in weapon good stats
    public int[] rangeAttack;
    public int[] rangeDefense;
    public int frontage;
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
