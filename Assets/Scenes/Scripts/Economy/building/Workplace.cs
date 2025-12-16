using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int id;
    public int countryId;
    private int provinceId;
    private int workplaceId;
    public string name;
    public WorkplaceType type;
    public int constructionCost; // IC cost / Time

    public List<GoodRequirement> goodConstructionCost = new List<GoodRequirement>();
    public List<GoodRequirement> maintenanceCost = new List<GoodRequirement>();

    private Dictionary<int, int> popAmmount = new Dictionary<int, int>();  // track wich popId own how many worker 

    public List<WorkerRequirment> workersRequirement; // track workers by type
    public Dictionary<int, int> owner;
    public int level;

    public int profit;
    public int cashBuffer;
    public int cashBufferMax;
    public int efficiency;

    public int poorStrataWage = 1;
    public int middleStrataWage = 2;

    public bool isOpen;
    public bool isUnderConstruction;

    public Workplace(
        int _provinceId, int _workplaceId, 
        int _countryId,
        List<GoodRequirement> _goodConstructionCost, 
        List<GoodRequirement> _maintenanceCost,
        Dictionary<int, int> _owner,
        List<WorkerRequirment> _workers,
        float wageMultiplier)
            {
                countryId = _countryId;
                provinceId = _provinceId;
                workplaceId = _workplaceId;
                goodConstructionCost = _goodConstructionCost;
                maintenanceCost = _maintenanceCost;
                owner = _owner;
                workersRequirement = _workers;
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

    public void HireWorker(int popId, int numberOfHired, PopJob type)
    {
        if (!popAmmount.ContainsKey(popId))
            popAmmount[popId] = 0;
            

        popAmmount[popId] += numberOfHired;

        WorkerRequirment wr = workersRequirement.Where(workersRequirement => workersRequirement.workerType == type).First();
        wr.curentMax.current += numberOfHired;
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

    public int GetNumberOfProducer()
    {
        int producer = 0;
        for (int i = 0; i < workersRequirement.Count; i++)
        {
            if (workersRequirement[i].workerType.type == "farmer")
            {
                producer += workersRequirement[i].curentMax.current;
            }
        }
        return 0;
    }
}