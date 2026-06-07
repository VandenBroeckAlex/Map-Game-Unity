using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandleWorkplacePay
{
   public List<IdNum> WorkplacesPay(DataRegistery dataRegistery)
    {
        List<IdNum> result = new List<IdNum>();

        List<WorkplaceInstance> wiList = dataRegistery.workplacesInstances;

        foreach (WorkplaceInstance wi in wiList) 
        {
            List<IdNum> data = wi.PayWorkers();
            result.AddRange(data);
        }
        return result;
    }
}
