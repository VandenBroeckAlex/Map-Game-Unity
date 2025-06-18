using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;

public class Building : MonoBehaviour
{
    public Good outputGood;
    public float baseOutputPerWorker;
    public List<GoodInput> inputGoods;
    public List<Pop> workers;
    public float efficiency;

    public float CalculateProduction()
    {
        // Scale by number of workers and efficiency
        return baseOutputPerWorker * workers.Count * efficiency;
    }
}

public class GoodInput
{
    public Good good;
    public float amountRequired;
}