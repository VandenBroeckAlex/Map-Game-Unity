using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using static MarketTransactionsObj;

public class PopulationSystem
{


    private List<Pop> allPops = new List<Pop>();


    private Dictionary<int, List<Pop>> popsByProvince = new Dictionary<int, List<Pop>>();
    private Dictionary<int, List<Pop>> popsByReligion = new Dictionary<int, List<Pop>>();

    public void AddPop(Pop pop)
    {
        allPops.Add(pop);
        AddToBucket(popsByProvince, pop.provinceId, pop);
        AddToBucket(popsByReligion, pop.religionId, pop);
    }

    public void RemovePop(Pop pop)
    {
        allPops.Remove(pop);
        RemoveFromBucket(popsByProvince, pop.provinceId, pop);
        RemoveFromBucket(popsByReligion, pop.religionId, pop);
    }


    /*
    public void ChangePopProvince(Pop pop, int newProvinceId)
    {
        // 1. Remove from the old bucket
        RemoveFromBucket(popsByProvince, pop.provinceId, pop);

        // 2. Update the pop's actual data
        pop.provinceId = newProvinceId;

        // 3. Add to the new bucket
        AddToBucket(popsByProvince, newProvinceId, pop);
    }
    */
    public List<Pop> GetPopsInProvince(int provinceId)
    {
        return popsByProvince.TryGetValue(provinceId, out var list) ? list : new List<Pop>();
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

    
    
