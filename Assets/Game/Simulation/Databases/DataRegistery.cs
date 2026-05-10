using System;
using System.Collections.Generic;
using System.Linq;
using static MarketTransactionsObj;



public class DataRegistery
{
    public string[] popStrata { get; set; } = new string[] { "Default"};
    public string[] goodTypes { get; set; } = new string[] { "Default" };
    public Good[] goodList { get; set; }
  
    public string[] countriesTag { get; set; } = new string[] { "Default" };
    public string[] provincesTag { get; set; } = new string[] { "Default" };
    public string[] terrainTypesTags { get; set; } = new string[] { "Default" };
    public TerrainType[] terrainTypes { get; set; } = new TerrainType[] { };
    public string[] climateTypesTags { get; set; } = new string[] { "Default" };
    public Dictionary<string, int> rgoTag = new Dictionary<string, int> 
    {
        { "Default", 0 },
    };
    //TODO To change initialisation of default
    private static readonly Culture _default = new Culture("default", "default");

    public Culture[] cultures = new Culture[] { _default };
    public PopJob[] popJobs;
    public Religion[] religionsDef;
    public Dictionary<string, GoodNeedMax[]> strataNeeds;
    public Dictionary<int, Pop> PopulationDict;

    public string[] provinceTag = new string[] { "Default" };
    public Dictionary<int, Tile> tiles = new Dictionary<int,Tile>();
    public Dictionary<int,Province> provinces = new Dictionary<int,Province>();
    public List<MapGraphNode> mapGraphNodes;
    public Dictionary<int, List<UnitNavigation>> mapUnitState; //province unit
    public Dictionary<int, UnitNavigation> mapUnitDict = new Dictionary<int, UnitNavigation>();
    public List<Building> buildings = new List<Building>();
    public Dictionary<int,Country> countryDict = new Dictionary<int, Country>();
    public List<Market> marketList = new List<Market>();

    //--- buffers ---
    //Response should be dispatch by response process, no need of buffer ? 
    public List<MarketBuyRequest> marketBuyRequests = new List<MarketBuyRequest>();
    public List<MarketBuyResponse> marketBuyResponseBuffer = new List<MarketBuyResponse>();
    public List<MarketSellRequest> marketSellRequestBuffer = new List<MarketSellRequest>();
    public List<MarketSellResponse> marketSellResponseBuffer = new List<MarketSellResponse>();
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
    public int GetProvinceIDByTag(string tag)
    {
        return GetIdByString(tag, provincesTag);
    }
    public int GetTerrainTypes(string tag)
    {
        return GetIdByString(tag, terrainTypesTags);
    }
    public TerrainType GetTerrainTypeById(int id)
    {
        return terrainTypes.ElementAtOrDefault(id);
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

    public Tile GetTileById(int Id)
    {
        if (tiles.TryGetValue(Id, out Tile tile))
        {
            return tile;
        }
        else
        {
            throw new Exception($"Tried to get un-present tile : ID({Id})");
        }
    }
    public int GetProvinceIdByTile(int Id)
    {
        Tile tile = GetTileById(Id);
        return tile.province;
    }


    public int GetCountryIdByTile(int tileId)
    {
        Tile tile = GetTileById(tileId);
        Province province = provinces[tile.province];
        return province.ownerId;
    }
    public MapGraphNode GetMapGraphNode(int id)
    {
        foreach(MapGraphNode node in mapGraphNodes)
        {
            if(node.GetProvinceId() == id)
            {
                return node;
            }
        }
            throw new Exception($"Try to get an un-existing graph node | id:{id}");
        
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
