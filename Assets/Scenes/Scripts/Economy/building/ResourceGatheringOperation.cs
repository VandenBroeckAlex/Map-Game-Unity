using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PopulationManager;


public class ResourceGatheringOperation  : IWorkspace, IProductionBuilding
{
    public Workplace workplace;
    public Production productionWorkplace;
    public Goods outputGoods;
    public int rgoRequirment;
    public Production Production;

    public ResourceGatheringOperation(Production _productionWorkplace,
        Goods _outputGoods)
    {
        productionWorkplace = _productionWorkplace;
        outputGoods = _outputGoods;
    }

    public void OnWorkerHired(int popId, int numberOfHired)
    {
        workplace.HireWorker(popId, numberOfHired);
    }


    //fire low skill labour first

    public event Action<int, int, int> WorkersFired;
    public void LayOffWorker(PopulationManager populationManager)
    {

        foreach(KeyValuePair<int, int> kvp in workplace.GetPopAmmount())
        {
            int id = kvp.Key;
            int totalAmmount= kvp.Value;

            //10% per call
            int NumFired = Mathf.Max(1, Mathf.CeilToInt(totalAmmount * 0.1f));


            workplace.LayOffWorker(id, NumFired);
            WorkersFired?.Invoke(id, NumFired, workplace.id);

        }
        

        //Notify pop manager
    }
  

    public void OutputGoods()
    {
        throw new System.NotImplementedException();
    }

    public void PayEmployees()
    {
        foreach (KeyValuePair<int, int> kvp in workplace.GetPopAmmount())
        {
            //Get pop type through PopManager

            // ammount  -> PopManager
        }
    }

    public void SetWages()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public void Degrade()
    {
        throw new System.NotImplementedException();
    }

    public void DestroyBuiding()
    {
        throw new System.NotImplementedException();
    }

    public void TakeLoan()
    {
        throw new System.NotImplementedException();
    }

    public void BuyMaintenanceGood()
    {
        throw new System.NotImplementedException();
    }

    
}
