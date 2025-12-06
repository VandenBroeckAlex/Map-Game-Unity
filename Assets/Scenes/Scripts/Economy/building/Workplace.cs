using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static Goods;
using static Pop_objects;


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
public class Workplace 
{
    private int provinceId;
    private int workplaceId;
    public string name;
    public WorkplaceType type;
    public int constructionCost; // IC cost
    public List<GoodRequirement> goodConstructionCost = new List<GoodRequirement>();
    public List<GoodRequirement> maintenanceCost = new List<GoodRequirement>();

    private Dictionary<int, int> popAmmount = new Dictionary<int, int>();

    public List<WorkerRequirment> workers;
    public int level;

    public int poorStrataWage = 1;
    public int middleStrataWage = 2;

    public Workplace(int _provinceId, int _workplaceId, List<GoodRequirement> _goodConstructionCost, List<GoodRequirement> _maintenanceCost, List<WorkerRequirment> _workers, float wageMultiplier)
    {
        provinceId = _provinceId;
        workplaceId = _workplaceId;
        goodConstructionCost = _goodConstructionCost;
        maintenanceCost = _maintenanceCost;
        workers = _workers;
        poorStrataWage = (int)(poorStrataWage * wageMultiplier);
        middleStrataWage = (int)(middleStrataWage * wageMultiplier);
    }

    public class WorkerRequirment
    {
        public PopJob workerType;
        public IntCurentMax curentMax;

        public float GetEmploymentPercent()
        {
            return ((curentMax.max / curentMax.current) / curentMax.max) * 100;
        }

    }

    


    public int GetProvinceId()
    {
        return provinceId;
    }

    public int GetWorkplaceId()
    {
        return workplaceId;
    }

    public Dictionary<int,int> GetPopAmmount()
    {
        return popAmmount;
    }

    public void HireWorker(int popId, int numberOfHired)
    {
        if (!popAmmount.ContainsKey(popId))
            popAmmount[popId] = 0;

        popAmmount[popId] += numberOfHired;

    }

    public void LayOffWorker(int popId, int numberOfLayedOff)
    {
        if (!popAmmount.ContainsKey(popId))
            throw new InvalidOperationException("pop not working here");


        if (popAmmount[popId] < numberOfLayedOff)
            throw new InvalidOperationException("pop does not have so much employee working here");

        popAmmount[popId] -= numberOfLayedOff;

        if(popAmmount[popId] == 0)
        {
            popAmmount.Remove(popId);
        }
    }


}