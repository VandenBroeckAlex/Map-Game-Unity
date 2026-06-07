
/*
 Pop response to a job offer
 */
public class JobApplication
{
    public int workplaceId;
    public int popId;
    public int popType;
    public int amount;
    public JobApplication(int workplaceId, int popId, int popType, int amount) {
        this.workplaceId = workplaceId; 
        this.popId = popId; 
        this.popType = popType;
        this.amount = amount;
    }
}

public struct PopFired
{
    public int workplaceId;
    public int popId;
    public int amount;
}