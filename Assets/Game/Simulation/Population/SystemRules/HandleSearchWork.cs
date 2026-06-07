/*
 Create a data object with pop workerID and how many available
 Pass it to buffer
 */

using System.Collections.Generic;
using System.Linq;

public class HandleWorkSearch
{
    public List<JobApplication> SearchWork(DataRegistery _registery)
    {
        List<JobApplication> result = new List<JobApplication>();

        Dictionary<int, Pop> popDictionary = _registery.GetPopNeedingJob();
        List<JobOffer> applications = _registery.jobOffersBuffer;


        foreach (KeyValuePair<int,Pop> pop in popDictionary)
        {
            List<JobOffer> offers = GetEligibleOffers(pop.Value.jobId, pop.Value.provinceId, applications);
            result.AddRange(PopHiredInWorkplace(pop.Value, offers));
        }
        return result;
    }

    //Get valid offer return them sorted by wage
    private List<JobOffer> GetEligibleOffers(int jobId,int provinceId, List<JobOffer> applications)
    {
        List<JobOffer> result = new List<JobOffer>();

        foreach (JobOffer app in applications){ 
            if(app.popType == jobId && app.provinceId == provinceId && app.openPositions > 0)
            {
                result.Add(app);
            }
        }
        result = result.OrderByDescending(jo => jo.wage).ToList();
        return result;
    }

    private List<JobApplication> PopHiredInWorkplace(Pop pop,List<JobOffer> offers)
    {
        List <JobApplication> result = new List<JobApplication>();
        foreach (JobOffer off in offers) 
        {
            int unemployed = pop.GetUnemployedNumber();

            if(unemployed < off.openPositions)
            {
                pop.HireInWorkplace(off.workplaceId,unemployed);
                off.openPositions -= unemployed;
                JobApplication response = new JobApplication(off.workplaceId,pop.id,pop.jobId, unemployed);
                result.Add(response);
                break;
            }
            else if(unemployed >= off.openPositions)
            {
                pop.HireInWorkplace(off.workplaceId, off.openPositions);
                JobApplication response = new JobApplication(off.workplaceId, pop.id, pop.jobId, off.openPositions);
                off.openPositions = 0;
                result.Add(response);
            }
        }
        return result;
    }
}

