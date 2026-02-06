using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Market_object;
using static PopulationManager;
using static Workplace;


public class ResourceGatheringOperation  : IWorkplace, IProductionBuilding
{
    public Workplace workplace;
    public Production production;
    public int outputGoodId;
    public int rgoRequirment;
    public Production Production;


    public event Action<int, int, int> WorkersFired; // delegate should be in manager

    public ResourceGatheringOperation(Workplace _workplace,Production _productionWorkplace, 
        int _outputGoodId)
    {
        workplace = _workplace;
        production = _productionWorkplace;
        outputGoodId = _outputGoodId;
    }

    public void OnWorkerHired(int popId, int numberOfHired, Pop.PopJob type)
    {
        workplace.HireWorker(popId, numberOfHired, type);
    }

    public void OnWorkerLeave(int popId, int numberOfLeaving)
    {
        workplace.LayOffWorkerByID(popId, numberOfLeaving);
    }


    //fire low skill labour first - first in last out ?
    // first in first out 
    //10% per call
    public List<IdNum> LayOffWorker()
    {
        List<IdNum> poplayedoff = new List<IdNum>();

        int numberToFire = (int)(workplace.GetNumberOfProducer() * 0.1f);

        return workplace.LayOfWorkerFIFO(numberToFire); 
    }
  

    //add higher grade worker after
    public MarketSellRequest SellRequest()
    {
        int numberOutputed = (workplace.GetNumberOfProducer() / 1000) * production.efficiency;// * (smallest numb inputGood);
        GoodSellRequest gsr = new GoodSellRequest(outputGoodId, numberOutputed);
        MarketSellRequest request = new MarketSellRequest(workplace.id,workplace.countryId, gsr);
        return request;
    }



    // popId => ammount
    public List<IdNum>? PayEmployees()
    {
        Debug.Log("RGO workplace call pay employee");
        return workplace.PayEmployees();
    }


    
    public List<IdNum>? PayOwners()
    {
        return workplace.PayOwner();
    }

    public void SetWages()
    {
        throw new System.NotImplementedException();
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
        // Higher Max employee, higher production efficiency ?
    }

    public void Degrade()
    {
        throw new System.NotImplementedException();
        // Lower Max employee, lower production efficiency ?
    }

    public void DestroyBuiding()
    {
        throw new System.NotImplementedException();
        //remove all employee  
        // kepp/sell stockpile
        //redistribute cashbuffer to owner
        //remove owner
    }

    public void TakeLoan()
    {
        throw new System.NotImplementedException();
        // for upgrade or construction(probably for pop), input good, first wages ?
    }

    public void BuyMaintenanceGood()
    {
        throw new System.NotImplementedException();
    }

    

    public void ReciveCash(int ammount)
    {
       workplace.cashBuffer += ammount;
    }

    public int GetWorkplaceId()
    {
        return workplace.id;
    }

    public int GetWorkAvailableByJobType(Pop.PopJob popJob)
    {
       WorkerTypeCurrentMax val = workplace.workersRequirement.Where(wr => wr.workerType.iD == popJob.iD).FirstOrDefault();
        
        if(val is null)
        {
            return 0;
        }
        else
        {
            return val.curentMax.max - val.curentMax.current;
        }
    }

    public int GetProvinceId()
    {
        return workplace.GetProvinceId();
    }
}
