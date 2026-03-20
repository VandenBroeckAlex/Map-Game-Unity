using System.Collections.Generic;
using UnityEngine;
using static ClimateTypeLoader;
using static TerrainTypeLoader;

public class LoaderDataRegistery
{
    string[] popStrata;
    string[] goodTypes;
    public Good[] goodList;
    public Dictionary<string, int> rgoTag;
    string[] countryTag;
    string[] provinceTag;
    public TerrainType[] terrainTypes;
    public string[] terrainTypesTags;
    public ClimateType[] climateTypes;
    public string[] climateTypesTags;

    public int GetId(string tag, string[] list)
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
}
