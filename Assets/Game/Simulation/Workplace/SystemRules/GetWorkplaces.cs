

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class GetWorkplaces
{
    public WorkplaceInstance ById(int id, List<WorkplaceInstance> instances)
    {
        return instances.Where(instance => instance.GetWorkplaceId() == id).FirstOrDefault();
    }
    public List<WorkplaceInstance> ByProvinceId(int id, List<WorkplaceInstance> instances)
    {
        return instances.Where(instance => instance.provinceId == id).ToList();
    }
    public List<WorkplaceInstance> ByTileId(int id, List<WorkplaceInstance> instances)
    {
        return instances.Where(instance => instance.tileId == id).ToList();
    }
    public List<WorkplaceInstance> ByCountryId(int id, List<WorkplaceInstance> instances)
    {
        return instances.Where(instance => instance.countryId == id).ToList();
    }
}
