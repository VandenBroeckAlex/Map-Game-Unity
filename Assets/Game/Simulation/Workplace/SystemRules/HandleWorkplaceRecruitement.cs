using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleWorkplaceRecruitement
{
    //hire
    public List<JobOffer> WorkplaceJobOffers(List<WorkplaceInstance> list, Dictionary<int,WorkplaceTemplate>templates)
    {
        List <JobOffer> jobOffer = new List <JobOffer>();
        foreach (WorkplaceInstance workplace in list) 
        { 
            templates.TryGetValue(workplace.TemplateId, out var template);
            if (workplace.CalculateEmploymentRatio(template) < 1)
            {
                jobOffer.Concat(workplace.JobOffers(template));
            }
        }
        return jobOffer;
    }

    public List<IdNum> WorkplaceFire(List<WorkplaceInstance> list, int ammountInPercent)
    {
        List<IdNum> result = new List<IdNum>();
        double percentFactor = ammountInPercent / 100.0;

        foreach (WorkplaceInstance workplace in list)
        {
            for (int i = workplace.currentWorkers.Count - 1; i >= 0; i--)
            {
                IdNum worker = workplace.currentWorkers[i];
                int ammountOffired = (int)Math.Ceiling(worker.num * percentFactor);

                if (ammountOffired > worker.num)
                {
                    ammountOffired = worker.num;
                }
                worker.num -= ammountOffired;

                if (ammountOffired > 0)
                {
                    result.Add(new IdNum(worker.id, ammountOffired));
                }
                if (worker.num <= 0)
                {
                    workplace.currentWorkers.RemoveAt(i);
                }
            }
        }
        return result;
    }
}
