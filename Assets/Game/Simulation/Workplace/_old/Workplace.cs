using System;
using System.Collections.Generic;
using System.Linq;




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
    public WorkplaceType type;
    public int definitionId;
    public int size;
    //list of pop type that can work here
    public int constructionCost; // IC cost / Time
    // currentmax construction cost ?
    //beeing buid every tick?

    public List<GoodRequirement> goodConstructionCost = new List<GoodRequirement>();
    public List<GoodRequirement> maintenanceGoods = new List<GoodRequirement>();

    private Dictionary<int, PopJobAssignment> popAmmount = new Dictionary<int, PopJobAssignment>();  // track wich popId own how many worker + type of pop

    public List<WorkerTypeCurrentMax> workersRequirement; // track workers curent/max by type  / TODO change this variable name
    public List<IdNum> owner; // Owner number + share in workplace ? or only for company ?
    public int level;

    public int profit;
    private int cashBuffer;
    private int cashBufferMax;
    public int efficiency;

    public int poorStrataWage = 100;
    public int middleStrataWage = 200;

    public bool isOpen;
    public bool isUnderConstruction;

    public Workplace(
        List<GoodRequirement> _goodConstructionCost, 
        List<GoodRequirement> _maintenanceCost,
        List<IdNum> _owner,
        float wageMultiplier,
        int definitionId)
        {
            goodConstructionCost = _goodConstructionCost;
            maintenanceGoods = _maintenanceCost;
            owner = _owner;
            workersRequirement = new List<WorkerTypeCurrentMax>();
            poorStrataWage = (int)(poorStrataWage * wageMultiplier);
            middleStrataWage = (int)(middleStrataWage * wageMultiplier);
            cashBuffer = 1000;
            this.definitionId = definitionId;
        }




    public Dictionary<int, PopJobAssignment> GetPopAmmount()
    {
        return popAmmount;
    }

    public void HireWorker(int popId, int numberOfHired, PopJob type)
    {
        if (!popAmmount.ContainsKey(popId))
            //To refactor
            //popAmmount[popId] = new PopJobAssignment(popId, numberOfHired, type.iD);


            popAmmount[popId].numWorker += numberOfHired;

        WorkerTypeCurrentMax wr = workersRequirement.Where(workersRequirement => workersRequirement.workerType == type).First();
        wr.curentMax.current += numberOfHired;
    }

    public void LayOffWorkerByID(int popId, int numberOfLayedOff)
    {
        if (!popAmmount.ContainsKey(popId))
            throw new InvalidOperationException("pop not working here");


        if (popAmmount[popId].numWorker < numberOfLayedOff)
            throw new InvalidOperationException("pop does not have so much employee working here");

        popAmmount[popId].numWorker -= numberOfLayedOff;

        if(popAmmount[popId].numWorker == 0)
        {
            popAmmount.Remove(popId);
        }
    }

    public List<IdNum> LayOfWorkerFIFO(int numberOfLayedOff)
    {
        int numLeft = numberOfLayedOff;
        List<IdNum> idNums = new List<IdNum>();

        foreach(KeyValuePair <int,PopJobAssignment> pop in popAmmount)
        {
            if(pop.Value.numWorker > numLeft)
            {
                //To refactor
                //WorkerTypeCurrentMax wr = workersRequirement.Where(workersRequirement => workersRequirement.workerType.iD == pop.Value.typeId).First();
                //wr.curentMax.current -= numLeft;
                //IdNum idNum = new(pop.Key, numLeft);
                //idNums.Add(idNum);
                //return idNums;
            }
            else if (pop.Value.numWorker <= numLeft)
            {
                //To refactor
                //WorkerTypeCurrentMax wr = workersRequirement.Where(workersRequirement => workersRequirement.workerType.iD == pop.Value.typeId).First();
                //numLeft -= pop.Value.numWorker;
                //wr.curentMax.current -= pop.Value.numWorker;
                //IdNum idNum = new(pop.Key, pop.Value.numWorker);
                //idNums.Add(idNum);
                //popAmmount.Remove(pop.Key);
            }
        }
        // if still numLeft  close workplace

        return idNums;

    }

    public List<IdNum> PayEmployees()
    {
        List<IdNum> info = new List<IdNum>();
        if (cashBuffer == 0)
            return null;
        foreach (KeyValuePair<int, PopJobAssignment> kvp in popAmmount)
        {


            if (kvp.Value.typeId == 1 && cashBuffer > 0)
            {
                int salary = (int)((kvp.Value.numWorker) * poorStrataWage);
                if(cashBuffer < salary)
                {
                    salary = cashBuffer;
                }
                cashBuffer =- salary;

                IdNum idNum = new IdNum(kvp.Key, salary);
                info.Add(idNum);
            }
            else if (kvp.Value.typeId == 4 && cashBuffer > 0)
            {
                int salary = (int)((kvp.Value.numWorker) * middleStrataWage);
                if (cashBuffer < salary)
                {
                    salary = cashBuffer;
                }
                cashBuffer = -salary;

                IdNum idNum = new IdNum(kvp.Key, salary);
                info.Add(idNum);
            }
            if(cashBuffer == 0)
            {
                break;
            }
        }
        return info;
    }

    //distribute surplus to owner   if owner have too much after buying all good it will give it to bank
    public List<IdNum>? PayOwner()
    {
        if (cashBuffer > cashBufferMax)
        {
            int amountForOwners = cashBuffer - cashBufferMax;
            int numberOfOwners = 0;
            List<IdNum> ownerPay = new List<IdNum>();
            foreach (IdNum idNum in owner) 
            {
                numberOfOwners += idNum.num;               
            }


            int ammountByOwner = amountForOwners / numberOfOwners;
            int remainder = amountForOwners % numberOfOwners;

            foreach (IdNum idNum in owner)
            {
                ownerPay.Add(new IdNum(idNum.id, idNum.num * ammountByOwner));
            }

            int index = 0;

            while (remainder > 0)
            {
                ownerPay[index].num++;
                remainder--;

                index++;
                if (index >= ownerPay.Count)
                    index = 0;
            }
            return ownerPay;
        }
        return null;
    }

    //TODO revise this (get by type or smth)
    public int GetNumberOfProducer()
    {
        int producer = 0;
        for (int i = 0; i < workersRequirement.Count; i++)
        {
            //if (workersRequirement[i].workerType.strata == "poor")
            //{
            //    producer += workersRequirement[i].curentMax.current;
            //}
        }
        return 0;
    }

    public int SmallestProducerWorkerRatio()
    {
        int workerRatio = 100;
        foreach (WorkerTypeCurrentMax workerType in workersRequirement)
        {
            int ratio = workerType.GetEmploymentPercent();
            if (ratio < workerRatio)
            {
                workerRatio = ratio;
            }
        }
        return workerRatio;
    }

    public int GetWorkAvailableByJobType(PopJob popJob)
    {
        foreach(WorkerTypeCurrentMax workers in workersRequirement)
        {
            if(workers.workerType == popJob)
            {
                return workers.curentMax.max - workers.curentMax.current;
            }
        }
        return 0;
    }

    public bool HaveAllMaintenanceGoods()
    {
        foreach(GoodRequirement req in maintenanceGoods)
        {
            if(req.maxNeed > req.stockpile)
            {
                return false;
            }
        }
        return true;
    }

    public class WorkerTypeCurrentMax
    {
        public PopJob workerType;
        public IntCurentMax curentMax;
        public int GetEmploymentPercent()
        {
            return (curentMax.current/curentMax.max) * 100;
        }

        public WorkerTypeCurrentMax(PopJob _workerType)
        {
            workerType = _workerType;
            curentMax = new IntCurentMax(0,1000);
        }
    }

    // track wich popId own how many worker + type of pop
    public class PopJobAssignment
    {
        public int popId;
        public int numWorker;
        public int typeId;

        public PopJobAssignment(int popId, int  numWorker, int typeId)
        {
            this.popId = popId;
            this.numWorker = numWorker;
            this.typeId = typeId;
        }
    }


}