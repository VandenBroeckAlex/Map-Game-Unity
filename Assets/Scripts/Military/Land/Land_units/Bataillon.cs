using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Bataillon 
{
    [SerializeField]
    public int id;
    public string name;
    public FloatCurentMax organisation;
    public FloatCurentMax moral;
    public IntCurentMax manPower;
    public IntCurentMax officer;
    public List<GoodRequirement> supply;
    public bool isSupport;
    //public List<> supply;
    //public Weight weight;
    // public float speed
    //public float coalConsumption;
    //public float armor;
    //public float softness; (%)
    public int range; // Should be in weapon good stats
    public int[] rangeAttack; //dictionnary
    public int[] rangeDefense; //dictionnary
    public int frontage;
    public bool isRouted = false;
    //public float airDefense;
    public float firepower;
    public int initiative;
    //public float hardAttack;
    // public float melee
    //public int  combatWidth;
    // public FCurentMax entrenchement
    //public level
    //public experience
    


    public float CurentSoftAttack(int range)
    {
        if (manPower.max == 0)
            return 0;

        return firepower * ((manPower.current / manPower.max) * (organisation.current/organisation.max) );
    }

    public int PercentManPower()
    {
        if (manPower.max == 0)
            return 0;

        return Mathf.RoundToInt((manPower.current / manPower.max) * 100f);
    }

  
}

public class BEBataillon 
{
    public Bataillon stats;
  
    public bool isAttacker;
    public bool haveMoved = false;
    public bool isDisengaging = false;
    public BEBataillon(Bataillon baseBataillon)
    {
        stats = baseBataillon;
    }
    public Bataillon ToBataillon()
    {
        return stats;
    }
}