using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataRegistery_Builder
{
    public string[] popStrata { get; set; }
    public string[] goodTypes { get; set; }
    public Good[] goodList { get; set; }

    public string[] countriesTag { get; set; }

    public string[] terrainTypesTags { get; set; }
    public TerrainType[] terrainTypes { get; set; } = new TerrainType[] { };
    public string[] climateTypesTags { get; set; }
    public Dictionary<string, int> rgoTag = new Dictionary<string, int>
    {
        { "Default", 0 },
    };
    //TODO To change initialisation of default
    private static readonly Culture _default;

    public Culture[] cultures = new Culture[] { _default };
    public PopJob[] popJobs;
    public Religion[] religionsDef;
    public Dictionary<string, GoodNeedMax[]> strataNeeds;
    public string[] provinceTag;
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
    public DataRegistery_Builder WithTileDictionary(Dictionary<int, Tile> tileDict)
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
    public DataRegistery_Builder WithRgoTag(Dictionary<string, int> rgo)
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
    public DataRegistery_Builder WithMapUnitDict(Dictionary<int, UnitNavigation> dict)
    {
        this.mapUnitDict = dict;
        return this;
    }

    public DataRegistery Build()
    {
        DataRegistery registery = new DataRegistery();
        string[] popStrata = new string[] { "Default" };
        registery.popStrata = popStrata.Concat(this.popStrata).ToArray();
        string[] goodtypes = new string[] { "Default" };
        registery.goodTypes = goodtypes.Concat(this.goodTypes).ToArray();

        Good[] goods = new Good[this.goodList.Length + 1];
        goods[0] = CreateDefaultGood();
        registery.goodList = goods.Concat(this.goodList).ToArray();
        string[] countryTags = new string[] { "Default" };
        registery.countriesTag = countryTags.Concat(this.countriesTag).ToArray();


        string[] terrainTypeTags = new string[] { "Default" };
        registery.terrainTypesTags = terrainTypeTags.Concat(this.terrainTypesTags).ToArray();

        TerrainType[] terrainTypes = new TerrainType[this.terrainTypes.Length + 1];
        terrainTypes[0] = CreateDefaultTerrainType();

        registery.terrainTypes = terrainTypes.Concat(this.terrainTypes).ToArray() ;

        throw new NotImplementedException();
    }

    private Good CreateDefaultGood()
    {

        Good defaultGood = new Good();
        defaultGood.id = 0;
        defaultGood.name = "Default";
        defaultGood.tag = "Default";
        defaultGood.type = 0;
        defaultGood.basePrice = 0;
        defaultGood.weight = 0;
        defaultGood.baseProductionModdifier = 0;
        defaultGood.iconPath = "default";
        defaultGood.color = "#808080";
        return defaultGood;
    }

    private TerrainType CreateDefaultTerrainType()
    {
        TerrainType defaultTerrain = new TerrainType();
        defaultTerrain.isLandType = true;
        defaultTerrain.tag = "def";
        defaultTerrain.name = "Default";
        defaultTerrain.movementCost = 1;
        return defaultTerrain;
    }

    private Culture CreateDefaultCulture()
    {
        string tag = "Default";
        Culture defaultCulture = new Culture(tag, tag);
        return defaultCulture;
    }

    private PopJob CreateDefaultPopJob()
    {
        string tag = "Default";
        PopJob defaultPopJob = new PopJob(tag, 0, tag);
        return defaultPopJob;
    }

    private Religion CreateDefaultReligion() {
        string tag = "Default";
        Religion defaultReligion = new Religion(tag, tag);
        return defaultReligion;
    }
}
