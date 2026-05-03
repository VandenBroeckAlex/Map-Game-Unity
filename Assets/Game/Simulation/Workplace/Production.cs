using GluonGui.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workplace;

public class Production 
{
    public int size;
    public int cashBuffer;
    public int cashBufferMax;
    public int efficiency;
    public int baseOutput;
    public int profit;
    public int producedGoodId;
    public List<GoodRequirement> inputGoods;
    public Production(
        int _cashBufferMax,
        int _efficiency
        ) 
    {
        cashBuffer = 0;
        cashBufferMax = _cashBufferMax;
        efficiency = _efficiency;
    }
    public bool requiresInput => inputGoods != null;

    public int GetsmallestInputGoodRatio(int size)
    {
        int result = 100;

        foreach (GoodRequirement good in inputGoods)
        {
            int ratio = (int)((good.stockpile * size) / (good.maxNeed * size)) * 100;
            if(result < ratio)
            {
                result = ratio;
            }
        }
        return result;
    }

    // if ratio = 100 remove all
    // if 0 remove none
    public void RemoveInputRatio(int ratio)
    {
        foreach (GoodRequirement good in inputGoods) 
        { 
            int toRemove = (good.maxNeed * ratio)/100;
            good.stockpile -= toRemove;
        }
    }
}
