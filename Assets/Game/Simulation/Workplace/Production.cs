using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class Production 
{

    public int cashBuffer;
    public int cashBufferMax;
    public int efficiency;
    

    public Production(
        int _cashBufferMax,
        int _efficiency
        ) 
    {
        cashBuffer = 0;
        cashBufferMax = _cashBufferMax;
        efficiency = _efficiency;
    }


}
