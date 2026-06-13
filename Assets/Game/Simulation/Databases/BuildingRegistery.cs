using NUnit.Framework;
using System.Collections.Generic;

public class BuildingRegistery
{
    private List<WorkplaceInstance> allWorkplace = new List<WorkplaceInstance>();
    private Dictionary<int, List<WorkplaceInstance>> workplaceByCountry = new Dictionary<int, List<WorkplaceInstance>>();
    private Dictionary<int, List<WorkplaceInstance>> workplaceByProvince = new Dictionary<int, List<WorkplaceInstance>>();
    private Dictionary<int, List<WorkplaceInstance>> workplaceByTile = new Dictionary<int, List<WorkplaceInstance>>();


    public void AddWorkplace(WorkplaceInstance workplace)
    {
        allWorkplace.Add(workplace);
        AddToBucket(workplaceByCountry, workplace.countryId, workplace);
        AddToBucket(workplaceByProvince, workplace.provinceId, workplace);
        AddToBucket(workplaceByTile, workplace.tileId, workplace);
    }

    public void RemoveWorkplace(WorkplaceInstance workplace)
    {
        allWorkplace.Remove(workplace);
        RemoveFromBucket(workplaceByCountry, workplace.countryId, workplace);
        RemoveFromBucket(workplaceByProvince, workplace.provinceId, workplace);
        RemoveFromBucket(workplaceByTile, workplace.tileId, workplace);
    }

    public List<WorkplaceInstance> GetAllPop()
    {
        return allWorkplace;
    }
    public List<WorkplaceInstance> GetWorkplaceInCountry(int countryId)
    {
        return workplaceByCountry.TryGetValue(countryId, out var list) ? list : new List<WorkplaceInstance>();
    }
    public List<WorkplaceInstance> GetWorkplaceInProvince(int provinceId)
    {
        return workplaceByProvince.TryGetValue(provinceId, out var list) ? list : new List<WorkplaceInstance>();
    }
    public List<WorkplaceInstance> GetWorkplaceInTile(int tileId)
    {
        return workplaceByTile.TryGetValue(tileId, out var list) ? list : new List<WorkplaceInstance>();
    }
  

    // ---  ---

    private void AddToBucket(Dictionary<int, List<WorkplaceInstance>> dict, int key, WorkplaceInstance pop)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = new List<WorkplaceInstance>();
        }
        dict[key].Add(pop);
    }
    private void RemoveFromBucket(Dictionary<int, List<WorkplaceInstance>> dict, int key, WorkplaceInstance pop)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Remove(pop);
        }
    }
}
