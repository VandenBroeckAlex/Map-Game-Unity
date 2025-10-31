using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBattleEngine : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        Tick_script.onTick += Turn;
    }

    // ------ Army info -------
    public List<Brigade> Attacker;
    public List<Brigade> Defender;

    public List<Bataillon> A_InField = new List<Bataillon>();
    public List<Bataillon> D_InField = new List<Bataillon>();

    public List<Bataillon> A_ReinforcementPool = new List<Bataillon>();
    public List<Bataillon> D_ReinforcementPool = new List<Bataillon>();

    //public General AttackerGeneral
    //public General DefenderGeneral
    // -----------------------


    // ------ Battlefield info -------
    public int range = 3;
    public int fieldFrontage;

    public List<Bataillon>[] battlefield;
    
    private void initializeBattleField()
    {
        //frontage = TileTerrain.frontagte;
        CreateBataillonPool();

        GetFieldRange();

        CreateField();

        PlaceTroopInField();
    }


   private void CreateBataillonPool()
    {
        foreach (var brigade in Attacker)
        {
            A_InField.AddRange(brigade.bataillons);
        }
        foreach (var brigade in Defender)
        {
            D_InField.AddRange(brigade.bataillons);
        }
    }

    private void CreateField()
    {
        battlefield = new List<Bataillon>[range];
        for (int i = 0; i < range; i++)
        {
            battlefield[i] = new List<Bataillon>();
        }
    }


    private void PlaceTroopInField()
    {
        //attacker 

        // check if isSupport

        //yes
        //position 0
        //check if some of bataillon frontage including the one to add is smaller than field frontage

        //no position 1


        //defender
        // check if isSupport

        //yes
        //position range -1
        //no position range -2 
    }

    public void PlaceBataillon(Bataillon b, int position)
    {
        battlefield[position].Add(b);
    }


    private void Turn()
    {

    }

    public void RangeAttack()
    {

    }
    public void AssaultAttack()
    {

    }
    private void GetFieldRange()
    {
        foreach(var bataillon in A_InField)
        {
            if(bataillon.range > range)
            {
                range = bataillon.range;
            }
        }
        foreach (var bataillon in D_InField)
        {
            if (bataillon.range > range)
            {
                range = bataillon.range;
            }
        }
    }


   

    public void MoveBataillon(Bataillon b, int from, int to)
    {
        battlefield[from].Remove(b);
        battlefield[to].Add(b);
    }

    public List<Bataillon> GetAtPosition(int position)
    {
        return battlefield[position];
    }


}
