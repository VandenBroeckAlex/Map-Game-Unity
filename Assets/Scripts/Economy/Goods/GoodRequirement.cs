using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodRequirement 
{
    public int Good_id;
    public int Stockpile;
    public int MaxNeed;
    GoodRequirement(int _good_id, int _stockpile, int _maxNeed)
    {
        Good_id = _good_id;
        Stockpile = _stockpile;
        MaxNeed = _maxNeed;
    }
}
