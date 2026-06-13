using System.Collections.Generic;


public class PopRegistery
{
    private Dictionary<int, string> strata;
    private Dictionary<int, PopJob> popJob;


    private List<Pop> allPops = new List<Pop>();

    // in a pop registery
    private Dictionary<int, List<Pop>> popsByProvince = new Dictionary<int, List<Pop>>();
    private Dictionary<int, List<Pop>> popsByReligion = new Dictionary<int, List<Pop>>();
    private Dictionary<int, List<Pop>> popsByCountry = new Dictionary<int, List<Pop>>();

    public void AddPop(Pop pop)
    {
        allPops.Add(pop);
        AddToBucket(popsByProvince, pop.provinceId, pop);
        AddToBucket(popsByReligion, pop.religionId, pop);
        AddToBucket(popsByCountry, pop.countryID, pop);
    }

    public void RemovePop(Pop pop)
    {
        allPops.Remove(pop);
        RemoveFromBucket(popsByProvince, pop.provinceId, pop);
        RemoveFromBucket(popsByReligion, pop.religionId, pop);
        RemoveFromBucket(popsByCountry, pop.countryID, pop);
    }

    public List<Pop> GetPopsInProvince(int provinceId)
    {
        return popsByProvince.TryGetValue(provinceId, out var list) ? list : new List<Pop>();
    }
    public List<Pop> GetPopInCountry(int countryId)
    {
        return popsByCountry.TryGetValue(countryId, out var list) ? list : new List<Pop>();
    }
    public List<Pop> GetAllPop()
    {
        return allPops;
    }

    // ---  ---

    private void AddToBucket(Dictionary<int, List<Pop>> dict, int key, Pop pop)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = new List<Pop>();
        }
        dict[key].Add(pop);
    }
    private void RemoveFromBucket(Dictionary<int, List<Pop>> dict, int key, Pop pop)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Remove(pop);
        }
    }
}
