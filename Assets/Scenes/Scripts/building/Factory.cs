using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : ProductionWorkplace
{
    public Dictionary<string, int> inputGoods;
    public Dictionary<string, int> outputGoods;

    //Type of production
    //Type of remuneration

    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Check Input
        if (type == WorkplaceType.Factory)
        {
            if (CanProduce(goodsStockpile, workerCount))
            {
                foreach (var input in inputGoods)
                    goodsStockpile[input.Key] -= input.Value;

                foreach (var output in outputGoods)
                    goodsStockpile[output.Key] += output.Value;
            }
        }
    }

    private bool CanProduce(Dictionary<string, int> goodsStockpile, int workerCount)
    {

        foreach (var input in inputGoods)
            if (!goodsStockpile.ContainsKey(input.Key) || goodsStockpile[input.Key] < input.Value)
                return false;
        return true;
    }
}
 