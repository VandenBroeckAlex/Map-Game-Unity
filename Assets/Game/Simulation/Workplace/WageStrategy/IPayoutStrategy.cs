using System.Collections.Generic;

public interface IPayoutStrategy
{
    public List<IdNum> Pay(int benefice, List<IdNum> currentWorkers, List<IdNum> owners);
    bool HaveSetWage();
    void SetWage(int wage, int popStrata);
    int LastWorkerCost();
}
