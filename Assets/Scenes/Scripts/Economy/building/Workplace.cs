using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;


public enum WorkplaceType
{
    ResourceGatheringOperation,
    Factory,
    Service,
    Military,
    Infrastructure,
    Unique
}

[System.Serializable]
public abstract class Workplace 
{
    public string name;
    public WorkplaceType type;
    public int constructionCost; // IC cost
    public Dictionary<string, int> maintenanceCost;
    public IntCurentMax Workers;
    public string workerType; // change to enum
    public int provinceId;

 
}