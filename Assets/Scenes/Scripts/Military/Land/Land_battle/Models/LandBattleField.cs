using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBattleField 
{
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
    public LandBattleField(List<Brigade> Attacker, List<Brigade> Defender)
    {
        this.Attacker = Attacker;  
        this.Defender = Defender;
    }
}
