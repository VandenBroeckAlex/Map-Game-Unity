using System.Collections.Generic;

public class Dime : IPayoutStrategy
{
    private int lastWorkerCost;
   
    public bool HaveSetWage() => false;
    public void SetWage(int wage, int popStrata)
    {
        // Because HaveSetWage = false
        //  Do Nothing
    }
    
    public List<IdNum> Pay(int benefice, List<IdNum> currentWorkers, List<IdNum> owners)
    {
      
        int totalWorkers = GetTotalPopCount(currentWorkers);
        int totalOwners = GetTotalPopCount(owners);

        if (benefice <= 0 || (totalWorkers == 0 && totalOwners == 0))
        {
            return new List<IdNum>();
        }

        int ownerPart = totalOwners > 0 ? benefice / 10 : 0;
        int workerPart = totalWorkers > 0 ? benefice - ownerPart : 0;
        lastWorkerCost = workerPart;

        if (totalWorkers == 0) ownerPart = benefice;


        List<IdNum> result = new List<IdNum>();
        result.AddRange(DistributeFunds(currentWorkers, workerPart, totalWorkers));
        result.AddRange(DistributeFunds(owners, ownerPart, totalOwners));

        return result;
    }
    public int LastWorkerCost()
    {
        return lastWorkerCost;
    }
    //Move this else where
    public static int GetTotalPopCount(List<IdNum> pops)
    {
        int total = 0;
        foreach (IdNum pop in pops)
        {
            total += pop.num;
        }
        return total;
    }

    public static List<IdNum> DistributeFunds(List<IdNum> pops, int totalFunds, int totalPopCount)
    {
        List<IdNum> payoutList = new List<IdNum>();
        if (totalFunds == 0 || totalPopCount == 0) return payoutList;

        int distributedAmount = 0;

        foreach (IdNum pop in pops)
        {
            int pay = (pop.num * totalFunds) / totalPopCount;
            distributedAmount += pay;

            payoutList.Add(new IdNum(pop.id, pay));
        }


        int remainder = totalFunds - distributedAmount;
        if (remainder > 0 && payoutList.Count > 0)
        {
            var firstPop = payoutList[0];
            payoutList[0] = new IdNum(firstPop.id, firstPop.num + remainder);
        }

        return payoutList;
    }



}

