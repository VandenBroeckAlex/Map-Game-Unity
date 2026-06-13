using System.Collections.Generic;
using static Dime;
public class ClassicWage : IPayoutStrategy
{
    int poorStrataWage;
    int middleClassStrataWage;
    int lastWorkerCost = 0;
    public bool HaveSetWage()
    {
        return true;
    }

    public int LastWorkerCost()
    {
        return lastWorkerCost;
    }


    //Strata wage handling
    public List<IdNum> Pay(int benefice, List<IdNum> currentWorkers, List<IdNum> owners)
    {
        int numberOfWorker = GetTotalPopCount(currentWorkers);

        lastWorkerCost = numberOfWorker * poorStrataWage;

        List <IdNum> result = new List<IdNum>();

        foreach (var worker in currentWorkers) 
        {
            result.Add(new IdNum(worker.id,(worker.num * poorStrataWage)/1000));
        }

        return result;
    }

    //TODO change this, it is ugly
    public void SetWage(int wage, int popStrata)
    {
        if(popStrata == 0)
        {
            poorStrataWage = wage;
        }
        if(popStrata == 1)
        {
            middleClassStrataWage = wage;
        }
    }
}
