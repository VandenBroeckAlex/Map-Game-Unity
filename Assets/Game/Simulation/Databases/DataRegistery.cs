using System;
using System.Collections.Generic;



public class DataRegistery
{
    public string[] popStrata { get; set; } = new string[] { "Default"};
    public string[] goodTypes { get; set; } = new string[] { "Default" };
    public Good[] goodList { get; set; }
  
    public string[] countriesTag { get; set; } = new string[] { "Default" };
    public string[] provincesTag { get; set; } = new string[] { "Default" };
    public string[] terrainTypesTags { get; set; } = new string[] { "Default" };
    public string[] climateTypesTags { get; set; } = new string[] { "Default" };
    public Dictionary<string, int> RgoTag = new Dictionary<string, int> 
    {
        { "Default", 0 },
    };
    public Culture[] cultures;
    public PopJob[] popJobs;
    public Religion[] religionsDef;
    public Dictionary<string, GoodNeedMax[]> strataNeeds;
    public string[] provinceTag = new string[] { "Default" };
    public Dictionary<int, Tile> tiles = new Dictionary<int,Tile>();
    public List<MapGraphNode> mapGraphNodes;

    public int GetPopStrataTagId(string popStrataTag)
    {
        return GetIdByString(popStrataTag, popStrata);
    }
    public int GetGoodTypeTagId(string goodTypeTag)
    {
        return GetIdByString(goodTypeTag, goodTypes);
    }
    public int GetGoodIdByTagId(string goodTag)
    {
        for (int i = 0; i < goodList.Length; i++)
        {
            if (goodList[i].tag == goodTag)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetRgoTagId(string tag)
    {
        if (RgoTag.ContainsKey(tag))
        {
            return RgoTag[tag];
        }
        else
        {
            return -1;
        }

    }
    public int GetCountryTagId(string tag)
    {
        return GetIdByString(tag, countriesTag);
    }
    public int GetProvinceID(string tag)
    {
        return GetIdByString(tag, provincesTag);
    }
    public int GetTerrainTypes(string tag)
    {
        return GetIdByString(tag, terrainTypesTags);
    }
    public int GetClimateTagId(string tag)
    {
        return GetIdByString(tag, climateTypesTags);
    }
    
    public int GetCultureId(string tag)
    {
        return GetIdByTag(tag, cultures);
    }

    public int GetPopJobId(string tag)
    {
        return GetIdByTag(tag, popJobs);
    }
    public int GetReligionId(string tag)
    {
        return GetIdByTag(tag, religionsDef);
    }
    //     public Dictionary<string, GoodNeedMax[]> strataNeeds;

    public GoodNeedMax[] GetGoodNeedsPerStrata(int jobId)
    {
        PopJob popJob = popJobs[jobId];
        string strata = popStrata[popJob.strata];
        string strataTag = strata;
        if (strataNeeds.TryGetValue(strataTag, out var goodNeeds))
        {
            return goodNeeds;
        }
        else
        {
            return null;
        }

    }
    public int GetProvinceId(string tag)
    {
       return GetIdByString(tag, provinceTag);
    }
    //provinceTags

    public Dictionary<int, int> GetWorkersDictionary(Dictionary<string, int> workers)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        foreach (KeyValuePair<string, int> kvp in workers)
        {
            int id = GetIdByTag(kvp.Key, popJobs);
            if (id < 0)
            {
                //throw new InvalidDataException(
                //$"the pop tag: {kvp.Key} is not valid. " +
                //$"While creating workplace definition");
            }
            else
            {
                result[id] = kvp.Value;
            }
        }
        return result;
    }

    public Dictionary<int, int> GetGoodDictionary(Dictionary<string, int> goods)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        foreach (KeyValuePair<string, int> kvp in goods)
        {
            int id = GetGoodIdByTagId(kvp.Key);
            if (id < 0)
            {
                //throw new InvalidDataException(
                //$"the pop tag: {kvp.Key} is not valid. " +
                //$"While creating workplace definition");
            }
            else
            {
                result[id] = kvp.Value;
            }
        }
        return result;

    }
    /*------------------------------*/
    private int GetIdByString(string tag, string[] list)
    {

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Equals( tag))
            {
                return i;
            }
        }
        return -1;
    }

    private int GetIdByTag<T>(string givenTag,T[] data) where T : IHaveTag
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].tag == givenTag)
            {
                return i;
            }
        }
        return -1;
    }

}
