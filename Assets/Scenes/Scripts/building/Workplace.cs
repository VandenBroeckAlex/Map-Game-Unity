using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;


public enum WorkplaceType
{
    ResourceGathering,
    Factory,
    Service,
    Military,
    Infrastructure,
    Unique
}

[System.Serializable]
public class Workplace
{
    public string name;
    public WorkplaceType type;
    public int constructionCost; // IC cost
    public Dictionary<string, int> maintenanceCost; 
    public Dictionary<string, int> inputGoods; 
    public Dictionary<string, int> outputGoods; 
    public FloatCurentMax maxWorkers;
    public float efficiency; // Affected by tech, workforce skill, etc.
    public int provinceId;
    public float cashBuffer;
    public float cashBufferMax;
    public Dictionary<int,int> owner; 

    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Check Input
        if(type == WorkplaceType.Factory)
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