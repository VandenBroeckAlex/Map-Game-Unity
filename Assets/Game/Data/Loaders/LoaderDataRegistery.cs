using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static ClimateTypeLoader;
using static CultureLoader;
using static ReligionLoader;
using static StrataNeedLoader;
using static TerrainTypeLoader;

public class LoaderDataRegistery
{
    public string[] popStrata { get; set; }
    public string[] goodTypes { get; set; }
    public Good[] goodList { get; set; }
    public Dictionary<string, int> rgoTag { get; set; }
    public string[] countriesTag { get; set; }
    public string[] provincesTag { get; set; }
    public string[] terrainTypesTags { get; set; }
    public string[] climateTypesTags { get; set; }
    public RunTimeCulture[] cultures;
    public RunTimePopJob[] popJobs;
    public RunTimeReligion[] religionsDef;
    public Dictionary<string, GoodNeedMax[]> strataNeeds;
    public string[] provinceTag;
    public Dictionary<int, Tile> tiles;
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
        if (rgoTag.ContainsKey(tag))
        {
            return rgoTag[tag];
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
    //cultures
    //popJobs
    //religionDef
    //strataNeeds
    //provinceTags

    public Dictionary<int, int> GetWorkersDictionary(Dictionary<string, int> workers)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        foreach (KeyValuePair<string, int> kvp in workers)
        {
            int id = GetIdByTag(popJobs, kvp.Key);
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

    private int GetIdByString(string tag, string[] list)
    {

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == tag)
            {
                return i;
            }
        }
        return -1;
    }

    private int GetIdByTag<T>(T[] data, string givenTag) where T : IHaveTag
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
