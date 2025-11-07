using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class LandBattleEngine : MonoBehaviour
{
    
    public bool battleOver = false;
    private void OnEnable()
    {
        Tick_script.onTick += BattleTurn;
    }

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
    private int baseDamage = 5;


    private void Start()
    {
        InitializeBattleField();
        for (int i = 0; i < 10; i++)
        {
            BattleTurn();
        }
            
        //return raport
    }

    private void BattleTurn()
    {
        //while(battleOver is false)
                
            List<BEBataillon> listBataillon = A_InField.Concat(D_InField).ToList()
            .OrderByDescending(b => b.stats.initiative).ToList();

            foreach (var bataillon in listBataillon)
            {
                BataillonTurn(bataillon);
            }
        ResetTurnFlags();
    }

    private void InitializeBattleField()
    {
        //frontage = TileTerrain.frontagte;
        CreateBataillonPool();

        GetFieldRange();
        Debug.Log($"The field have a range of {range}");
        CreateField();

        InitializeTroopInField();

    }


   private void CreateBataillonPool()
    {
        foreach (var brigade in Attacker)
        {
            foreach(var bataillon in brigade.bataillons)
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
        for (int i = A_ReinforcementPool.Count - 1; i>= 0;  i--)
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

    
        for (int i = D_ReinforcementPool.Count -1; i >= 0; i--)
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
                    PlaceBataillon(bataillon, range-1);

                    D_ReinforcementPool.RemoveAt(i);
                    D_InField.Add(bataillon);
                }

            }
            //no position range -2 
            else if (bataillon is not null && bataillon.stats.isSupport == false)
            {
                int frontageLeft = GetFrontageInField(range - 2);
                if (frontageLeft + bataillon.stats.frontage  <= fieldFrontage)
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

    

    private void BataillonTurn(BEBataillon bataillon)
    {
        //get by initiative
       
        
            Debug.Log($"----  {bataillon.stats.name}'s turn ! -------");
            int bataillonPosition = GetBataillonPosition(bataillon);
            if (bataillon is not null)
            {
                
                int targetRow = GetTargetRow(bataillonPosition, bataillon.stats.range, bataillon.isAttacker);
                //Debug.Log($"targetRow = {targetRow}");
                //Debug.Log($"bataillonPosition : {bataillonPosition}, isAttacker : {bataillon.isAttacker}, targetRow : {targetRow}");
                //if targetRow == -1 N
                if(targetRow == -1 && bataillon.haveMoved is false)
                {
                    Debug.Log("Ennemi out of range");                    //advance
                    if(bataillon.isAttacker is true)
                    MoveBataillon(bataillon, bataillonPosition, bataillonPosition + 1);
                    else
                    MoveBataillon(bataillon, bataillonPosition, bataillonPosition - 1);
                }
                else if(bataillon.isAttacker is true && targetRow > bataillonPosition + 1)
                {
                    ResultRangeAttack(bataillon, battlefield[targetRow][0], targetRow - bataillonPosition);
                }
                else if (bataillon.isAttacker is false && targetRow < bataillonPosition -1)
                {
                    ResultRangeAttack(bataillon, battlefield[targetRow][0], targetRow - bataillonPosition);
                }
                else if (targetRow == bataillonPosition + 1)
                {
                    Debug.Log("Hand to hand battle");
                }        
            


            //GetAtPosition()
        }

        // advance
        // range dmg
        //check if one of the 2 have routed
        //reinforcment

    }

    public BEBataillon ResultRangeAttack(BEBataillon attaker, BEBataillon defender,int range)
    {
        for(int i = 0; i < attaker.stats.rangeAttack[0]; i++)
        {
            float damage = 0;
            //if both have dice
            if (defender.stats.rangeDefense[0] >= i)
            {
                int attackerRoll = UnityEngine.Random.Range(1, 7);
                int defenderRoll = UnityEngine.Random.Range(1, 7);
                Debug.Log($"Attack roll : {attackerRoll} / Defender roll : {defenderRoll}");
                if(attackerRoll > defenderRoll)
                {
                    //damage 10%
                    damage = attaker.stats.CurentSoftAttack(range) * 0.1f * baseDamage;
                }
            }
            else
            {
                //damage 40%
                 damage = attaker.stats.CurentSoftAttack(range) * 0.4f * baseDamage;
            }
            defender.stats.manPower.current -= (int)damage;
            Debug.Log($"{(int)damage} damges deal by {attaker.stats.name} to {defender.stats.name}");
        }

        return defender;
    }
    public void AssaultAttack()
    {

    }
    private void GetFieldRange()
    {
        foreach(var bataillon in A_ReinforcementPool)
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

    // When a brigade leave an attack or a defense to change tile
    private void DisEngage()
    {

    }

    private int GetBataillonPosition(BEBataillon bataillon)
    {
        for (int i = 0; i < battlefield.Length; i++)
        {
            if (battlefield[i].Contains(bataillon))
            {
                return i; // Found the position
            }
        }
        return -1; // Not on the battlefield
    }
    private void MoveBataillon(BEBataillon b, int from, int to)
    {
        battlefield[from].Remove(b);
        battlefield[to].Add(b);
        b.haveMoved = true;
        Debug.Log($"bataillon {b.stats.name} moved from {from} to {to}");
        BataillonTurn(b);
    }

    private List<BEBataillon> GetAtPosition(int position)
    {
        return battlefield[position];
    }

    public void ResetTurnFlags()
    {
        List<BEBataillon> listBataillon = A_InField.Concat(D_InField).ToList()
           .OrderByDescending(b => b.stats.initiative).ToList();
        foreach (var b in listBataillon)
        {
            b.haveMoved = false;
        }
    }

    private int GetFrontageInField(int field)
    {
       int fieldFrontage = 0;

        foreach(var bataillon in battlefield[field]) 
        {
            fieldFrontage += bataillon.stats.frontage;
        }
        return fieldFrontage;
    }

    private int GetTargetRow(int firingPosition, int range, bool isAttaker)
    {
        if (isAttaker)
        {
            for(int i = 1; i <= range ; i++)
            {
                if(i + firingPosition >= battlefield.Count())
                {
                    break;
                }
                if (battlefield[firingPosition + i].Count > 0 && battlefield[firingPosition + i][0].isAttacker is false)
                {
                    return firingPosition + i;
                }
            }
        }
        else
        {
            for (int i = 1; i <= range; i++)
            {
                if (firingPosition - i <= 0)
                {
                    break;
                }
                if (battlefield[firingPosition - i].Count > 0 && battlefield[firingPosition - i][0].isAttacker is true)
                {
                    return firingPosition - i;
                }
            }
        }

            return -1;
    }
}
