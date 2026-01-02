using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBattleField 
{
    public List<Brigade> attacker;
    public List<Brigade> defender;

    public List<BEBataillon> a_InField = new List<BEBataillon>();
    public List<BEBataillon> d_InField = new List<BEBataillon>();

    public List<BEBataillon> a_ReinforcementPool = new List<BEBataillon>();
    public List<BEBataillon> d_ReinforcementPool = new List<BEBataillon>();

    //public General AttackerGeneral
    //public General DefenderGeneral
    //   -----------------------


    // ------ Battlefield info -------
    public int fieldRange;
    public int fieldFrontage;
    public List<BEBataillon>[] battlefield;
    public LandBattleField(List<Brigade> Attacker, List<Brigade> Defender)
    {
        this.attacker = Attacker;  
        this.defender = Defender;
    }
}
