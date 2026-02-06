using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitializeLandBattle 
{
     
  

    // ------ Army info -------
    private List<Brigade> attacker;
    private List<Brigade> defender;

    private List<BEBataillon> a_InField = new List<BEBataillon>();
    private List<BEBataillon> d_InField = new List<BEBataillon>();

    private List<BEBataillon> a_ReinforcementPool = new List<BEBataillon>();
    private List<BEBataillon> d_ReinforcementPool = new List<BEBataillon>();

    //public General AttackerGeneral
    //public General DefenderGeneral
    //   -----------------------


    // ------ Battlefield info -------
    private int range = 3;
    private int fieldFrontage;


    private List<BEBataillon>[] battlefield;
    //   -----------------------
    // ---- Batle Info for json -------

    public LandBattleField InitializeBattleField(List<Brigade> _Attacker, List<Brigade> _Defender, int _fieldFrontage)
    {
        LandBattleField NewBattlefield = new LandBattleField(_Attacker, _Defender);
        attacker = _Attacker;
        defender = _Defender;
        fieldFrontage = _fieldFrontage;
        //frontage = TileTerrain.frontagte;
        CreateBataillonPool();

        GetFieldRange();
        Debug.Log($"The field have a range of {range}");
        Debug.Log($"The field have a frontage of {fieldFrontage}");
        CreateField();

        InitializeTroopInField();

        NewBattlefield.battlefield = battlefield;
        NewBattlefield.fieldRange = range;
        NewBattlefield.fieldFrontage = fieldFrontage;
        NewBattlefield.a_InField = a_InField;
        NewBattlefield.d_InField = d_InField;
        NewBattlefield.a_ReinforcementPool = a_ReinforcementPool;
        NewBattlefield.d_ReinforcementPool = d_ReinforcementPool;
        return NewBattlefield;
            
    }


    private void CreateBataillonPool()
    {
        foreach (var brigade in attacker)
        {
            foreach (var bataillon in brigade.bataillons)
            {
                BEBataillon newbat = new BEBataillon(bataillon);
                newbat.isAttacker = true;
                a_ReinforcementPool.Add(newbat);
            }

        }
        foreach (var brigade in defender)
        {
            foreach (var bataillon in brigade.bataillons)
            {
                BEBataillon newbat = new BEBataillon(bataillon);
                newbat.isAttacker = false;
                d_ReinforcementPool.Add(newbat);
            }
        }
    }

    private void CreateField()
    {
        battlefield = new List<BEBataillon>[range];
        for (int i = 0; i < range; i++)
        {
            battlefield[i] = new List<BEBataillon>();
        }
    }


    private void InitializeTroopInField()
    {
        //Keep track if frontage is full
        //bool fieldFrontageFilled = false;
        //bool SupportFrontageFilled = false;
        //attacker 
        for (int i = a_ReinforcementPool.Count - 1; i >= 0; i--)
        {
            var bataillon = a_ReinforcementPool[i];
            // check if isSupport
            if (bataillon is not null && bataillon.stats.isSupport == true)
            {
                //yes

                //check if some of bataillon frontage including the one to add is smaller than field frontage
                int frontageLeft = GetFrontageInField(0);
                if (bataillon.stats.frontage + frontageLeft <= fieldFrontage)
                {
                    //position 0
                    PlaceBataillon(bataillon, 0);

                    a_ReinforcementPool.RemoveAt(i);
                    a_InField.Add(bataillon);
                }

            }
            //no position 1
            else if (bataillon is not null && bataillon.stats.isSupport == false)
            {
                int frontageLeft = GetFrontageInField(1);
                if (bataillon.stats.frontage + frontageLeft <= fieldFrontage)
                {
                    PlaceBataillon(bataillon, 1);
                    a_ReinforcementPool.RemoveAt(i);
                    a_InField.Add(bataillon);
                }
            }
        }
        //defender
        // check if isSupport


        for (int i = d_ReinforcementPool.Count - 1; i >= 0; i--)
        {
            var bataillon = d_ReinforcementPool[i];
            // check if isSupport
            if (bataillon is not null && bataillon.stats.isSupport == true)
            {
                //yes

                //check if some of bataillon frontage including the one to add is smaller than field frontage
                int frontageLeft = GetFrontageInField(range - 1);
                if (bataillon.stats.frontage + frontageLeft <= fieldFrontage)
                {
                    //position range -1
                    PlaceBataillon(bataillon, range - 1);

                    d_ReinforcementPool.RemoveAt(i);
                    d_InField.Add(bataillon);
                }

            }
            //no position range -2 
            else if (bataillon is not null && bataillon.stats.isSupport == false)
            {
                int frontageLeft = GetFrontageInField(range - 2);
                if (frontageLeft + bataillon.stats.frontage <= fieldFrontage)
                {
                    PlaceBataillon(bataillon, range - 2);
                    d_ReinforcementPool.RemoveAt(i);
                    d_InField.Add(bataillon);
                }
            }
        }

    }

    public void PlaceBataillon(BEBataillon b, int position)
    {
        battlefield[position].Add(b);
    }


    private void GetFieldRange()
    {
        foreach (var bataillon in a_ReinforcementPool)
        {
            if (bataillon.stats.range > range)
            {
                range = bataillon.stats.range;
            }
        }
        foreach (var bataillon in d_ReinforcementPool)
        {
            if (bataillon.stats.range > range)
            {
                range = bataillon.stats.range;
            }
        }

        range += 1; //So troop on front line are at range
    }

    private int GetFrontageInField(int field)
    {
        int fieldFrontage = 0;

        foreach (var bataillon in battlefield[field])
        {
            fieldFrontage += bataillon.stats.frontage;
        }
        return fieldFrontage;
    }

    
}


