using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PopulationManager;


public class ResourceGatheringOperation  : IWorkspace, IProductionBuilding
{
    public Workplace workplace;
    public Production production;
    public Goods outputGoods;
    public int rgoRequirment;
    public Production Production;


    public event Action<int, int, int> WorkersFired;

    public ResourceGatheringOperation(Production _productionWorkplace, 
        Goods _outputGoods)
    {
        production = _productionWorkplace;
        outputGoods = _outputGoods;
    }

    public void OnWorkerHired(int popId, int numberOfHired, Pop_objects.PopJob type)
    {
        workplace.HireWorker(popId, numberOfHired, type);
    }

    public void OnWorkerLeave(int popId, int numberOfLeaving)
    {
        workplace.LayOffWorker(popId, numberOfLeaving);
    }


    //fire low skill labour first

    public void LayOffWorker()
    {

        foreach(KeyValuePair<int, int[]> kvp in workplace.GetPopAmmount())
        {
            int id = kvp.Key;
            int totalAmmount= kvp.Value[0];

            //10% per call
            int NumFired = Mathf.Max(1, Mathf.CeilToInt(totalAmmount * 0.1f));


            workplace.LayOffWorker(id, NumFired);
            WorkersFired?.Invoke(id, NumFired, workplace.id);

        }
        

        //Notify pop manager
    }
  

    public void OutputGoods()
    {
        int numberOutputed = (workplace.GetNumberOfProducer() / 1000) * production.efficiency;// * (smallest numb inputGood);
        // Market sell
        // Add response
    }

    // popId => ammount
    public void PayEmployees()
    {
        List<int[]> info = new List<int[]>();

        foreach (KeyValuePair<int, int[]> kvp in workplace.GetPopAmmount())
        {
            if( kvp.Value[1] == 0)
            {

                info.Add(new int[]{ kvp.Key, (int)((kvp.Value[0] / 1000) * workplace.poorStrataWage)});
            }
            else if (kvp.Value[1] == 2)
            {
                info.Add(new int[] { kvp.Key, (int)((kvp.Value[0] / 1000) * workplace.middleStrataWage)});
            }
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
