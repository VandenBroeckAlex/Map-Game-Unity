using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoaderBootstrap
{
    public void InitializeSimulation()
    {
        // 1) validate json
        JsonValidator validator = new JsonValidator();
        // 2) load def
        CultureLoader cultureLoader = new CultureLoader();
        PopJobLoader popJobLoader = new PopJobLoader();
        ReligionLoader religionLoader = new ReligionLoader();
        GoodTypeLoader goodTypeLoader = new GoodTypeLoader();
        // 3) load scenario data
        PopLoader popLoader = new PopLoader();
        GoodLoader goodLoader = new GoodLoader();
        // 4) export a redy sim object


        string goodDefJson = GetJsonDefFromFile("GoodDef");
        Dictionary<int,string> goodsType = goodTypeLoader.Deserialize_goodsType(goodDefJson);
    
        string cultureDefJson = GetJsonDefFromFile("cultureDef");
        Dictionary<int,CultureLoader.RunTimeCulture> cultureDef = cultureLoader.DeserializeCultures(cultureDefJson);

        string PopJobDefJson = GetJsonDefFromFile("GoodDef");
        PopJobDeserializeResult pjdr = popJobLoader.Deserialize_PopJob(PopJobDefJson);

        string ReligionDefJson = GetJsonDefFromFile("ReligionDef");
        Dictionary<int,ReligionLoader.RunTimeReligion> religionsDef = religionLoader.DeserializeReligions(ReligionDefJson);
    }
    public string GetJsonDefFromFile(string fileName)
    {
        string filePath = Path.Combine(
         Application.dataPath, 
         "Game",
         "StreamingAssets",
         "BaseGame",
         "Def",
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
