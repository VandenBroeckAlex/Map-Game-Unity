using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class LandBattleEngine : MonoBehaviour
{
    
    public bool battleOver = false;
    private void OnEnable()
    {
        // The battle manager will call that
        TickScript.onTick += BattleTurn;
    }

    // ------ Army info -------
    public List<Brigade> attacker;
    public List<Brigade> defender;

    public List<BEBataillon> a_InField = new List<BEBataillon>();
    public List<BEBataillon> d_InField = new List<BEBataillon>();

    public List<BEBataillon> a_ReinforcementPool = new List<BEBataillon>();
    public List<BEBataillon> d_ReinforcementPool = new List<BEBataillon>();


    private int a_troop_lost = 0;
    private int d_troop_lost = 0;

    //public General AttackerGeneral
    //public General DefenderGeneral
    //   -----------------------


    // ------ Battlefield info -------
    LandBattleField _landbattlefield;
    public int range = 3;
    public int fieldFrontage = 35;
    public List<BEBataillon>[] battlefield;
    //   -----------------------
    // ---- Batle Info for json -------
    private int baseDamage = 5;


    private void Start()
    {
        InitializeLandBattle initialiser = new InitializeLandBattle();
        _landbattlefield = initialiser.InitializeBattleField(attacker, defender, fieldFrontage);

        a_InField = _landbattlefield.a_InField;
        d_InField = _landbattlefield.d_InField;
        a_ReinforcementPool = _landbattlefield.a_ReinforcementPool;
        d_ReinforcementPool = _landbattlefield.d_ReinforcementPool;
        battlefield = _landbattlefield.battlefield;

        Debug.Log($"{a_InField.Count} : A_InField");
        Debug.Log($"{d_InField.Count} : D_InField");
        Debug.Log($"{a_ReinforcementPool.Count} : A_ReinforcementPool");
        Debug.Log($"{d_ReinforcementPool.Count} : D_ReinforcementPool");



        initialiser = null;

        for (int i = 0; i < 10; i++)
        {
            BattleTurn();
        }
            
        //return raport
    }

    private void BattleTurn()
    {
        //reinforcement here
       
        
        bool attackerRouting = IsRouting(a_InField);
        bool defenderRouting = IsRouting(d_InField);

        if (attackerRouting || defenderRouting)
        {
            EndBattle();
        }


        List<BEBataillon> listBataillon = a_InField.Concat(d_InField).ToList()
        .OrderByDescending(b => b.stats.initiative).ToList();
        Debug.Log(listBataillon.Count +"bataillons");
        foreach (var bataillon in listBataillon)
        {
            BataillonTurn(bataillon);
        }
        ResetTurnFlags();
    }

    private bool IsRouting(List<BEBataillon> army)
    {
        foreach(var bataillon in army)
        {
            if(bataillon.isDisengaging == false)
            {
                return false;
            }
        }
        return true;
    }


    private void EndBattle()
    {
        Debug.Log("The battle is over !");
        Debug.Log($"the Attacker have lost {a_troop_lost} men.");
        Debug.Log($"the Defender have lost {d_troop_lost} men.");
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
                    int index = SelectRandomBataillonInRow(targetRow);
                    ResultRangeAttack(bataillon, battlefield[targetRow][index], targetRow - bataillonPosition,true);
                }
                else if (bataillon.isAttacker is false && targetRow < bataillonPosition -1)
                {
                    int index = SelectRandomBataillonInRow(targetRow);
                    ResultRangeAttack(bataillon, battlefield[targetRow][0], targetRow - bataillonPosition, false);
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

    public BEBataillon ResultRangeAttack(BEBataillon damageDealer, BEBataillon damageReceiver,int range, bool is_attacker)
    {
        for(int i = 0; i < damageDealer.stats.rangeAttack[0]; i++)
        {
            float damage = 0;
            //if both have dice
            if (damageReceiver.stats.rangeDefense[0] >= i)
            {
                int attackerRoll = UnityEngine.Random.Range(1, 7);
                int defenderRoll = UnityEngine.Random.Range(1, 7);
                Debug.Log($"Attack roll : {attackerRoll} / Defender roll : {defenderRoll}");
                if(attackerRoll > defenderRoll)
                {
                    //damage 10%
                    damage = damageDealer.stats.CurentSoftAttack(range) * 0.1f * baseDamage;
                }
            }
            else
            {
                //damage 40%
                 damage = damageDealer.stats.CurentSoftAttack(range) * 0.4f * baseDamage;
            }
            damageReceiver.stats.manPower.current -= (int)damage;

            if(is_attacker == true)
            {
                d_troop_lost += (int)damage;
            }
            else
            {
                a_troop_lost += (int)damage;
            }
                Debug.Log($"{(int)damage} damges deal by {damageDealer.stats.name} to {damageReceiver.stats.name}");
        }

        return damageReceiver;
    }
    public void AssaultAttack()
    {

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
        List<BEBataillon> listBataillon = a_InField.Concat(d_InField).ToList()
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

    private int SelectRandomBataillonInRow(int row)
    {
        int max = battlefield[row].Count() ;
        int index = UnityEngine.Random.Range(0, max);
        Debug.Log($"max index is {max} , the chosen index is {index}");
        return index;
    }
}
