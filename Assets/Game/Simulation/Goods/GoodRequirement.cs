using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodRequirement 
{
    public int good_id;
    public int stockpile;
    public int maxNeed;
    public GoodRequirement(int _good_id, int _stockpile, int _maxNeed)
    {
        good_id = _good_id;
        stockpile = _stockpile;
        maxNeed = _maxNeed;
    }
}
