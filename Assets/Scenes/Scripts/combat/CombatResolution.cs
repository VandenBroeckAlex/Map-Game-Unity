using System;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class CombatResolution : MonoBehaviour
{

    List<Bataillon> attackerList;
    List<Bataillon> defenderList;

    //constructor
    CombatResolution(List<Bataillon> attacker, List<Bataillon> defender)
    {
        attackerList = attacker;
        defenderList = defender;
    }

    public void CombatTurn()
    {
        //0)assign unit randomly 
        for (int i = 0; i < attackerList.Count; i++)
        {
            Bataillon attacker = attackerList[i];
            Bataillon target = defenderList[UnityEngine.Random.Range(0, defenderList.Count)];
            //1) shot fire
            int attackerDamage = Convert.ToInt32(Math.Floor(attacker.softAttack * (attacker.manPower.current / attacker.manPower.max) * (attacker.organisation.current / attacker.organisation.max)));
            int targetDamage = Convert.ToInt32(Math.Floor(target.softAttack * (target.manPower.current / target.manPower.max) * (target.organisation.current / target.organisation.max)));


            //2)If hit strength lose between 1 - 2 org between 1 - 3
            //strength lost 
            target.manPower.current -= attackerDamage * UnityEngine.Random.Range(1, 2);
            attacker.manPower.current -= targetDamage * UnityEngine.Random.Range(1, 2);
        }
    }

}
