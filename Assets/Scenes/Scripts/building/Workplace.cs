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
    public string Name;
    public WorkplaceType Type;
    public int ConstructionCost; // IC cost
    public Dictionary<string, int> MaintenanceCost; 
    public Dictionary<string, int> InputGoods; // e.g., "Iron": 2
    public Dictionary<string, int> OutputGoods; // e.g., "Steel": 1
    public int MaxWorkers;
    public float Efficiency; // Affected by tech, workforce skill, etc.
    public int provinceId;
    public float cashBuffer;
    public float cashBufferMax;
    public Dictionary<int,int> owner; 

    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Check Input
        if(Type == WorkplaceType.Factory)
        {
            if (CanProduce(goodsStockpile, workerCount))
            {
                foreach (var input in InputGoods)
                    goodsStockpile[input.Key] -= input.Value;

                foreach (var output in OutputGoods)
                    goodsStockpile[output.Key] += output.Value;
            }
        }
        
    }

    private bool CanProduce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        
        foreach (var input in InputGoods)
            if (!goodsStockpile.ContainsKey(input.Key) || goodsStockpile[input.Key] < input.Value)
                return false;
        return true;
    }
}