
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ClimateTypeLoader;
using static CountryLoader;
using static CultureLoader;
using static ProvincesLoader;
using static StrataNeedLoader;
using static TerrainTypeLoader;
using static WorkplaceLoader;


public class LoaderBootstrap
{
    private string definitionPath = Path.Combine(
         Application.dataPath,
         "Game",
         "StreamingAssets",
         "BaseGame",
         "Def");

    private string runtimePath = Path.Combine(
         Application.dataPath,
         "Game",
         "StreamingAssets",
         "BaseGame");

    public void InitializeSimulation()
    {
        LoaderDataRegistery registery = new LoaderDataRegistery();
        // TODO handler choice
        IResolutionErrorHandler errorHandler = new ThrowErrorHandler();

     

        // 1) validate json
        GoodJsonValidator validator = new GoodJsonValidator();
        // 2) load def
        CultureLoader cultureLoader = new CultureLoader();
        PopJobLoader popJobLoader = new PopJobLoader();
        ReligionLoader religionLoader = new ReligionLoader();
        GoodTypeLoader goodTypeLoader = new GoodTypeLoader();
        StrataLoader strataLoder = new StrataLoader();
        
        // 3) load scenario data

        GoodLoader goodLoader = new GoodLoader(registery);
        StrataNeedLoader strataNeedLoader = new StrataNeedLoader();
        CountryLoader countryLoader = new CountryLoader();
        ProvincesLoader provincesLoader = new ProvincesLoader();
        TerrainTypeLoader terrainTypeLoader = new TerrainTypeLoader();
        ClimateTypeLoader climateTypeLoader = new ClimateTypeLoader();
        
        TileLoader tileLoader = new TileLoader();

        PopLoader popLoader = new PopLoader();
        // 4) export a redy sim object


        string goodDefJson = GetJsonStringFromFile(definitionPath,"GoodType");
        string[] goodsTypes = goodTypeLoader.Deserialize_goodsType(goodDefJson, errorHandler);
        registery.goodTypes = goodsTypes;

        string cultureDefJson = GetJsonStringFromFile(definitionPath,"cultureDef");
        RunTimeCulture[] cultureDef = cultureLoader.DeserializeCultures(cultureDefJson, errorHandler);
        registery.cultures = cultureDef;

        //Todo deserialize strata
        string stratadefJson = GetJsonStringFromFile(definitionPath, "PopStrataDef");
        string[] strata = strataLoder.DeserializeStrata(stratadefJson);
        registery.popStrata = strata;

        string PopJobDefJson = GetJsonStringFromFile(definitionPath,"PopJobDef");
        RunTimePopJob[] popJobs = popJobLoader.Deserialize_PopJob(PopJobDefJson, strata);
        registery.popJobs = popJobs;

        string ReligionDefJson = GetJsonStringFromFile(definitionPath,"ReligionDef");
        ReligionLoader.RunTimeReligion[] religionsDef = religionLoader.DeserializeReligions(ReligionDefJson);
        registery.religionsDef = religionsDef;

        string goodJson = GetJsonStringFromFile(definitionPath, "GoodDef");
        GoodLoader.GoodLoadedData goodData = goodLoader.Load_goods(goodJson);
        registery.goodList = goodData.goodList;

        string strataNeedsJson = GetJsonStringFromFile(definitionPath, "StrataNeedDef");
        Dictionary<string, GoodNeedMax[]> strataNeeds = strataNeedLoader.DeserializeStrataNeeds(strataNeedsJson, strata, goodData.goodList);
        registery.strataNeeds = strataNeeds;

        //deserialize country
        string countryDefJson = GetJsonStringFromFile(definitionPath, "CountryDef");
        CountryLoaderData countryData = countryLoader.DeserializeCountries(countryDefJson);
        registery.countriesTag = countryData.countriesTag;

        string provincePath = Path.Combine(runtimePath, "Provinces");
        string provinceJson = GetJsonStringFromFile(provincePath, "Provinces");
        ProvinceData provinceData = provincesLoader.LoadProvince(provinceJson, countryData.countriesTag);
        registery.provincesTag = provinceData.provinceTag;

        string tileDataPath = Path.Combine(runtimePath, "Tiles");
        string terrainTypesJson = GetJsonStringFromFile(tileDataPath, "TerrainTypes");
        TerrainTypesData terrainTypes = terrainTypeLoader.DeserializeTerrainTypeDef(terrainTypesJson);
        registery.terrainTypesTags = terrainTypes.tags;

        string climateTypeJson = GetJsonStringFromFile(tileDataPath, "ClimateType");
        ClimateTypeData climateTypesData = climateTypeLoader.deserializeClimateType(climateTypeJson);
        registery.climateTypesTags = climateTypesData.climateTypesTags;

        string tileDataJson = GetJsonStringFromFile(tileDataPath, "TilesData");
        Dictionary<int,Tile> tiles = tileLoader.DeserializeTiles(tileDataJson, goodData.rgoTag, terrainTypes.tags,countryData.countriesTag, provinceData.provinceTag, climateTypesData.climateTypesTags);

        string popFilePath = Path.Combine(runtimePath, "Population");
        string runTimePopJson = GetJsonStringFromFile(popFilePath,"population");
        List<Pop> listPop = popLoader.Deserialize_Pop(runTimePopJson, popJobs, cultureDef, religionsDef, goodsTypes, provinceData.provincesList);

        string workplaceDefFilePath = Path.Combine(runtimePath, "Workplaces");
        string workplacesDefJson = GetJsonStringFromFile(workplaceDefFilePath, "workplacesDef");
        WorkplaceLoader workplaceLoader = new WorkplaceLoader(registery);
        List<DefinitionWorkplace> workplaceDefinition = workplaceLoader.DeserializeWorkplaces(workplacesDefJson);
    }

    public string GetJsonStringFromFile(string dataPath,string fileName)
    {
        string filePath = Path.Combine(
         dataPath,
         $"{fileName}.json"
     );

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }

        Debug.LogError("JSON file not found at: " + filePath);
        return null;
    }
}
