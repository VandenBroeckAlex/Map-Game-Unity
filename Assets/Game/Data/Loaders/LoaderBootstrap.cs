
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static CultureLoader;


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
        // 1) validate json
        JsonValidator validator = new JsonValidator();
        // 2) load def
        CultureLoader cultureLoader = new CultureLoader();
        PopJobLoader popJobLoader = new PopJobLoader();
        ReligionLoader religionLoader = new ReligionLoader();
        GoodTypeLoader goodTypeLoader = new GoodTypeLoader();
        StrataLoader strataLoder = new StrataLoader();
        // 3) load scenario data
        PopLoader popLoader = new PopLoader();
        GoodLoader goodLoader = new GoodLoader();
        // 4) export a redy sim object


        string goodDefJson = GetJsonFromFile(definitionPath,"GoodType");
        string[] goodsTypes = goodTypeLoader.Deserialize_goodsType(goodDefJson);
         
        string cultureDefJson = GetJsonFromFile(definitionPath,"cultureDef");
        RunTimeCulture[] cultureDef = cultureLoader.DeserializeCultures(cultureDefJson);

        //Todo deserialize strata
        string stratadefJson = GetJsonFromFile(definitionPath, "PopStrataDef");
        string[] strata = strataLoder.DeserializeStrata(stratadefJson);

        string PopJobDefJson = GetJsonFromFile(definitionPath,"PopJobDef");
        RunTimePopJob[] pjdr = popJobLoader.Deserialize_PopJob(PopJobDefJson, strata);

        string ReligionDefJson = GetJsonFromFile(definitionPath,"ReligionDef");
        ReligionLoader.RunTimeReligion[] religionsDef = religionLoader.DeserializeReligions(ReligionDefJson);

        string goodJson = GetJsonFromFile(definitionPath, "GoodDef");
        goodLoader.Load_goods(goodJson, goodsTypes);

        string popFilePath = Path.Combine(runtimePath, "Population");
        string runTimePopJson = GetJsonFromFile(popFilePath,"population");
        List<Pop> listPop = popLoader.Deserialize_Pop(runTimePopJson, pjdr, cultureDef, religionsDef, goodsTypes);
    }

    public string GetJsonFromFile(string dataPath,string fileName)
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
