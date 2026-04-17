using System;
using System.Collections.Generic;
using UnityEngine;

public class DataRegistery_Builder
{
    public string[] popStrata { get; set; } = new string[] { "Default" };
    public string[] goodTypes { get; set; } = new string[] { "Default" };
    public Good[] goodList { get; set; }

    public string[] countriesTag { get; set; } = new string[] { "Default" };

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
    public string[] provinceTag = new string[] { "Default" };
    public Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();

    public List<MapGraphNode> mapGraphNodes;
    public Dictionary<int, List<UnitNavigation>> mapUnitState; //tile unit
    public Dictionary<int, UnitNavigation> mapUnitDict = new Dictionary<int, UnitNavigation>();

    public DataRegistery_Builder WithPopStrata(string[] strata)
    {
        this.popStrata = strata;
        return this;
    }
    public DataRegistery_Builder WithGoodType(string[] goodType)
    {
        this.goodTypes = goodType;
        return this;
    }
    public DataRegistery_Builder WithGoodList(Good[] goods)
    {
        this.goodList = goods;
        return this;
    }
    public DataRegistery_Builder WithProvinceTag(string[] provinceTags)
    {
        this.provinceTag = provinceTags;
        return this;
    }
    public DataRegistery_Builder WithTileDictionary(Dictionary<int,Tile> tileDict)
    {
        this.tiles = tileDict;
        return this;
    }
    public DataRegistery_Builder WithTerrainTypes(TerrainType[] terrainTypes)
    {
        this.terrainTypes = terrainTypes;
        return this;
    }
    public DataRegistery_Builder WithClimateTypes(string[] climateTypes)
    {
        this.climateTypesTags = climateTypes;
        return this;
    }
    public DataRegistery_Builder WithRgoTag(Dictionary<string,int> rgo)
    {
        this.rgoTag = rgo;
        return this;
    }
    public DataRegistery_Builder WithCultures(Culture[] cultures)
    {
        this.cultures = cultures;
        return this;
    }
    public DataRegistery_Builder WithPopJobs(PopJob[] popJobs)
    {
        this.popJobs = popJobs;
        return this;
    }
    public DataRegistery_Builder WithReligions(Religion[] religions)
    {
        this.religionsDef = religions;
        return this;
    }
    public DataRegistery_Builder WithStrataNeeds(Dictionary<string, GoodNeedMax[]> strataNeeds)
    {
        this.strataNeeds = strataNeeds;
        return this;
    }
    public DataRegistery_Builder WithMapGraphNodes(List<MapGraphNode> nodes)
    {
        this.mapGraphNodes = nodes;
        return this;
    }
    public DataRegistery_Builder WithMapUnitsStat(Dictionary<int, List<UnitNavigation>> state)
    {
        this.mapUnitState = state;
        return this;
    }
    public DataRegistery_Builder WithMapUnitDict(Dictionary<int,UnitNavigation> dict)
    {
        this.mapUnitDict = dict;
        return this;
    }

    public DataRegistery Build()
    {
        throw new NotImplementedException();
    }
}
