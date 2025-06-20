using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;


public enum BuildingType
{
    ResourceProduction,
    Manufacturing,
    Service,
    Military,
    Infrastructure,
    Unique
}

[System.Serializable]
public class Building
{
    public string Name;
    public BuildingType Type;
    public int ConstructionCost;
    public int MaintenanceCost;
    public int ConstructionTime; // in days
    public Dictionary<string, int> InputGoods; // e.g., "Iron": 2
    public Dictionary<string, int> OutputGoods; // e.g., "Steel": 1
    public int MaxWorkers;
    public float Efficiency; // Affected by tech, workforce skill, etc.
    public bool IsUnlocked;

    public virtual void Produce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        // Simplified example
        if (CanProduce(goodsStockpile, workerCount))
        {
            foreach (var input in InputGoods)
                goodsStockpile[input.Key] -= input.Value;

            foreach (var output in OutputGoods)
                goodsStockpile[output.Key] += output.Value;
        }
    }

    private bool CanProduce(Dictionary<string, int> goodsStockpile, int workerCount)
    {
        if (workerCount < MaxWorkers * 0.5f) return false; // Minimum workforce
        foreach (var input in InputGoods)
            if (!goodsStockpile.ContainsKey(input.Key) || goodsStockpile[input.Key] < input.Value)
                return false;
        return true;
    }
}