using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static CountriesManager;
using static Pop_objects;


public class PopJobDeserializer 
{
    
   List<PopJob> list = new List<PopJob>();

    string jsonPath = FilePath.PopJob;

    public List<PopJob> Deserialize()
    {
        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            list = JsonConvert.DeserializeObject<List<PopJob>>(jsonText);

            return list;
        }
        else
        {
            Debug.LogError($"json not found at " + jsonPath);
        }
        return list;
    }

}
