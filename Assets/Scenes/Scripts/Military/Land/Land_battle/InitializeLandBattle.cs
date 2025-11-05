using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitializeLandBattle : MonoBehaviour
{
     
  

    // ------ Army info -------
    public List<Brigade> Attacker;
    public List<Brigade> Defender;

    public List<BEBataillon> A_InField = new List<BEBataillon>();
    public List<BEBataillon> D_InField = new List<BEBataillon>();

    public List<BEBataillon> A_ReinforcementPool = new List<BEBataillon>();
    public List<BEBataillon> D_ReinforcementPool = new List<BEBataillon>();

    //public General AttackerGeneral
    //public General DefenderGeneral
    //   -----------------------


    // ------ Battlefield info -------
    public int range = 3;
    public int fieldFrontage;


    public List<BEBataillon>[] battlefield;
    //   -----------------------
    // ---- Batle Info for json -------

    private LandBattleField InitializeBattleField(List<Brigade> _Attacker, List<Brigade> _Defender)
    {
        LandBattleField NewBattlefield = new LandBattleField(_Attacker, _Defender);
        Attacker = _Attacker;
        Defender = _Defender;

        //frontage = TileTerrain.frontagte;
        CreateBataillonPool();

        GetFieldRange();
        Debug.Log($"The field have a range of {range}");
        CreateField();

        InitializeTroopInField();

        NewBattlefield.battlefield = battlefield;
        NewBattlefield.range = range;
        NewBattlefield.fieldFrontage = fieldFrontage;
        NewBattlefield.A_InField = A_InField;
        NewBattlefield.D_InField = D_InField;
        NewBattlefield.A_ReinforcementPool = A_ReinforcementPool;
        NewBattlefield.D_ReinforcementPool = D_ReinforcementPool;
        return NewBattlefield;
            
    }


    private void CreateBataillonPool()
    {
        foreach (var brigade in Attacker)
        {
            foreach (var bataillon in brigade.bataillons)
            {
                BEBataillon newbat = new BEBataillon(bataillon);
                newbat.isAttacker = true;
                A_ReinforcementPool.Add(newbat);
            }

        }
        foreach (var brigade in Defender)
        {
            foreach (var bataillon in brigade.bataillons)
            {
                BEBataillon newbat = new BEBataillon(bataillon);
                newbat.isAttacker = false;
                D_ReinforcementPool.Add(newbat);
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
        for (int i = A_ReinforcementPool.Count - 1; i >= 0; i--)
        {
            var bataillon = A_ReinforcementPool[i];
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

                    A_ReinforcementPool.RemoveAt(i);
                    A_InField.Add(bataillon);
                }

            }
            //no position 1
            else if (bataillon is not null && bataillon.stats.isSupport == false)
            {
                int frontageLeft = GetFrontageInField(1);
                if (bataillon.stats.frontage + frontageLeft <= fieldFrontage)
                {
                    PlaceBataillon(bataillon, 1);
                    A_ReinforcementPool.RemoveAt(i);
                    A_InField.Add(bataillon);
                }
            }
        }
        //defender
        // check if isSupport


        for (int i = D_ReinforcementPool.Count - 1; i >= 0; i--)
        {
            var bataillon = D_ReinforcementPool[i];
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

                    D_ReinforcementPool.RemoveAt(i);
                    D_InField.Add(bataillon);
                }

            }
            //no position range -2 
            else if (bataillon is not null && bataillon.stats.isSupport == false)
            {
                int frontageLeft = GetFrontageInField(range - 2);
                if (frontageLeft + bataillon.stats.frontage <= fieldFrontage)
                {
                    PlaceBataillon(bataillon, range - 2);
                    D_ReinforcementPool.RemoveAt(i);
                    D_InField.Add(bataillon);
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
        foreach (var bataillon in A_ReinforcementPool)
        {
            if (bataillon.stats.range > range)
            {
                range = bataillon.stats.range;
            }
        }
        foreach (var bataillon in D_ReinforcementPool)
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


